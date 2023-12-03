using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Yandex;
using Pulumi.Yandex.Inputs;

namespace InfoDeploy;

public static class Program
{
    public static async Task<int> Main()
    {
        return await Pulumi.Deployment.RunAsync(() =>
        {
            var saDeployer = new IamServiceAccount("sa-bucket-deployer");
            saDeployer.AssignRole("admin");

            var deployerKey = saDeployer.CreateStaticKey("deployer");

            var bucket = CreateStorageBucket(deployerKey);
            var saUploader = CreateSaToUploadFunctions();

            var sa = new IamServiceAccount("sa-for-func");

            sa.AssignRole("logging.viewer");
            sa.AssignRole("logging.reader");
            sa.AssignRole("functions.viewer");
            sa.AssignRole("functions.functionInvoker");
            sa.AssignRole("ymq.reader");
            sa.AssignRole("ymq.writer");

            var queue = new MessageQueue(
                "test",
                new MessageQueueArgs
                {
                    Name = "test",
                    AccessKey = deployerKey.AccessKey,
                    SecretKey = deployerKey.SecretKey,
                });
            var saStaticKey = sa.CreateStaticKey("in-func");

            var uploaderKey = saUploader.CreateStaticKey("uploader");

            var info = CreateFunction(
                "info",
                GetEnvironmentVariable("UPLOAD_INFOFUNC_PATH"),
                "YandexCloud.Examples.CloudFunctions.Info.Handler,InfoFunc",
                ( "FUNC_NAME", "info"),
                ("FOLDER_ID", GetEnvironmentVariable("YC_FOLDER_ID")));

            var qrCoder = CreateFunction(
                "qrCoder",
                GetEnvironmentVariable("UPLOAD_QRCODERFUNC_PATH"),
                "YandexCloud.Examples.CloudFunctions.QrCoder.Handler,QRCoderFunc",

                // Secrets should be stored in LockBox, not in env vars
                ("BOT_TOKEN", GetEnvironmentVariable("BOT_TOKEN")),
                ("API_KEY", GetEnvironmentVariable("API_KEY")));

            var queueReceiver = CreateFunction(
                "queueReceiver",
                GetEnvironmentVariable("UPLOAD_QUEUERECEIVER_PATH"),
                "YandexCloud.Examples.CloudFunctions.QueueReceiver.Handler,QueueReceiver");

            var queueSender = CreateFunction(
                "queueSender",
                GetEnvironmentVariable("UPLOAD_QUEUESENDER_PATH"),
                "YandexCloud.Examples.CloudFunctions.QueueSender.Handler,QueueSender",
                ("QUEUE_URL", queue.Id), //!!! Id contains URL
                ("AWS_ACCESS_KEY_ID", saStaticKey.AccessKey),
                ("AWS_SECRET_ACCESS_KEY", saStaticKey.SecretKey));

            var handler = CreateFunction(
                "handler",
                GetEnvironmentVariable("UPLOAD_HANDLERS_PATH"),
                "YandexCloud.Examples.CloudFunctions.Handlers.AsyncHandler,Handlers");

            var handlerSdk = CreateFunction(
                "handlerSdk",
                GetEnvironmentVariable("UPLOAD_HANDLERSDK_PATH"),
                "YandexCloud.Examples.CloudFunctions.HandlerSdk.Handler,HandlerSdk");

            new FunctionTrigger("queue-trigger",
                                new FunctionTriggerArgs()
                                {
                                    Function = new FunctionTriggerFunctionArgs()
                                    {
                                        Id = queueReceiver.Id,
                                        ServiceAccountId = sa.Id,
                                    },
                                    MessageQueue = new FunctionTriggerMessageQueueArgs()
                                    {
                                        BatchSize = "10",
                                        QueueId = queue.Arn, // !!! Id==Arn
                                        ServiceAccountId = sa.Id,
                                        BatchCutoff = "1",
                                    },
                                });

            return new Dictionary<string, object?>
            {
                ["infoId"] = info.Id.Apply(id => id),
                ["qrcoderId"] = qrCoder.Id.Apply(id => id),
                ["queueReceiverId"] = queueReceiver.Id.Apply(id => id),
                ["queueSenderId"] = queueSender.Id.Apply(id => id),
                ["handlerSenderId"] = handler.Id.Apply(id => id),
                ["handlerSdkSenderId"] = handlerSdk.Id.Apply(id => id),
            };

            Function CreateFunction(string name, string path, string entrypoint, params (string Name, Input<string> Value)[] additionalVars)
            {
                var hash = CalculateHash(path);
                var funcSources = bucket.SaveObject(Path.GetFileName(path), path, hash, uploaderKey);

                name = name.ToLowerInvariant();

                var envVars = new InputMap<string>();

                foreach (var envVar in additionalVars)
                {
                    envVars.Add(envVar.Name, envVar.Value);
                }

                var func = new Function(name, new FunctionArgs
                {
                    Name = name,
                    Package = new FunctionPackageArgs
                    {
                        BucketName = funcSources.Bucket,
                        ObjectName = funcSources.Key,
                    },
                    Runtime = "dotnet8",
                    ExecutionTimeout = "5",
                    UserHash = hash,
                    Entrypoint = entrypoint,
                    Memory = 128,
                    Environment = envVars,
                    ServiceAccountId = sa.Id,
                });

                return func;
            }
        });
    }

    private static IamServiceAccount CreateSaToUploadFunctions()
    {
        var saUploader = new IamServiceAccount("sa-uploader");

        saUploader.AssignRole("storage.editor");
        return saUploader;
    }

    private static StorageBucket CreateStorageBucket(IamServiceAccountStaticAccessKey deployerKey)
    {
        return new StorageBucket("func-sources",
             new StorageBucketArgs
             {
                 AccessKey = deployerKey.AccessKey,
                 SecretKey = deployerKey.SecretKey,
             });
    }

    private static string CalculateHash(string path)
    {
        using var md5 = MD5.Create();
        using var file = File.OpenRead(path);
        return Convert.ToHexString(md5.ComputeHash(file));
    }

    private static string GetEnvironmentVariable(string name) =>
        Environment.GetEnvironmentVariable(name) ??
        throw new InvalidOperationException($"Missing «{name}» environment variable");
}
