using System.Text.Json;
using YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver;

public sealed class Handler
{
    public Response FunctionHandler(MessageRequest request)
    {
        Console.WriteLine(JsonSerializer.Serialize(request));

        return new Response(200, "Ok");
    }
}
