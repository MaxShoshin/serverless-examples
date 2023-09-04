namespace YandexCloud.Examples.CloudFunctions.Handlers;

public sealed class AsyncHandlerNoResult
{
    /// <summary>
    /// Метод должен обязательно быть async.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task FunctionHandler(HttpRequest request)
    {
        await Task.Yield();
    }
}
