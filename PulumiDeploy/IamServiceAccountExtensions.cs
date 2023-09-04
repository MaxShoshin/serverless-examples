using Pulumi;
using Pulumi.Yandex;

namespace InfoDeploy;

internal static class IamServiceAccountExtensions
{
    public static void AssignRole(this IamServiceAccount sa, string role)
    {
        new ResourcemanagerFolderIamMember(
            role,
            new ResourcemanagerFolderIamMemberArgs
            {
                FolderId = sa.FolderId,
                Role = role,
                Member = sa.Id.Apply(id => $"serviceAccount:{id}"),
            });
    }

    public static IamServiceAccountStaticAccessKey CreateStaticKey(this IamServiceAccount sa, string name)
    {
        return new IamServiceAccountStaticAccessKey(
            $"aws-{name}",
            new IamServiceAccountStaticAccessKeyArgs
            {
                ServiceAccountId = sa.Id,
            });
    }
}
