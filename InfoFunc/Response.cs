using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.Info;

public sealed class Response
{
    public Response(int statusCode, string body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; }

    [JsonPropertyName("body")]
    public string Body { get; }
}
