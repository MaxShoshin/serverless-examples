namespace YandexCloud.Examples.CloudFunctions.Handlers;

public sealed class AsyncHandler
{
    /// <summary>
    /// Метод должен обязательно быть async.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> FunctionHandler(HttpRequest request)
    {
        await Task.Yield();

        return new Response(200, "OK");
    }
}

