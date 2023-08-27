namespace YandexCloud.Examples.CloudFunctions.Handlers;

public sealed class SyncHandler
{
    public Response FunctionHandler(HttpRequest request)
    {
        return new Response(200, "OK");
    }
}
