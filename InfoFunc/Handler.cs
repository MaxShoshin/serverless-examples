using System.Diagnostics;
using System.Text.Json;
using BenchmarkDotNet.Environments;

using Hardware.Info;
using Yandex.Cloud;
using Yandex.Cloud.Logging.V1;

namespace YandexCloud.Examples.CloudFunctions.Info;

public sealed class Handler
{
    private static readonly JsonSerializerOptions s_optIndented = new () { WriteIndented = true };

    public async Task<Response> FunctionHandler(JsonElement request)
    {
        var view = new View(
            JsonSerializer.Serialize(request, s_optIndented),
            GetHardwareInfo(),
            BenchmarkEnvironmentInfo.GetCurrent(),
            diskInfo: ProcessOutput("bash", "-c ls"),
            await GetLogs(GetEnvironmentVariable("FUNC_NAME")));

        return new Response(200, view.TransformText());
    }

    private HardwareInfo GetHardwareInfo()
    {
        var hardwareInfo = new HardwareInfo();
        hardwareInfo.RefreshAll();
        return hardwareInfo;
    }

    private async Task<string> GetLogs(string funcName)
    {
        // Note, we create SDK without specifing any information about running credentials.
        // So, in Function it will use Account which is used for running function
        var sdk = new Sdk();

        var logGroups = await sdk.Services.Logging.LogGroupService.ListAsync(new ListLogGroupsRequest()
        {
            FolderId = GetEnvironmentVariable(
                "FOLDER_ID"),
        });

        var logRequest = new ReadRequest();
        logRequest.Criteria = new Criteria();

        //Assume default group first, should pass via Env vars
        logRequest.Criteria.LogGroupId = logGroups.Groups.First().Id;

        var logResponse = await sdk.Services.Logging.LogReadingService.ReadAsync(logRequest);
        var logs = string.Join(Environment.NewLine, logResponse.Entries
                                   .OrderByDescending(x => x.Timestamp)
                                   .Select(x => x.Message));
        return logs;
    }

    private Process StartProcess(string cmd, string args)
    {
        return Process.Start(new ProcessStartInfo(cmd, args)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        }) ?? throw new InvalidOperationException("Unable to start process");
    }

    private string ProcessOutput(string cmd, string args )
    {
        using var process = StartProcess(cmd, args);
        using StreamReader standardOutput = process.StandardOutput;

        process.WaitForExit();
        return standardOutput.ReadToEnd().Trim();
    }

    private static string GetEnvironmentVariable(string name) =>
        Environment.GetEnvironmentVariable(name) ??
        throw new InvalidOperationException($"Missing «{name}» environment variable");
}
