using Pulumi.Yandex;

namespace InfoDeploy;

internal static class StorageBacketExtensions
{
    public static StorageObject SaveObject(
        this StorageBucket bucket,
        string key,
        string path,
        string hash,
        IamServiceAccountStaticAccessKey saStaticKey)
    {
        return new StorageObject($"{key}_{hash}", new StorageObjectArgs
        {
            AccessKey = saStaticKey.AccessKey,
            SecretKey = saStaticKey.SecretKey,
            Bucket = bucket.Id,
            Key = key,
            Source = path,
        });
    }
}
