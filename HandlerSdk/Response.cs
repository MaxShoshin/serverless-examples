using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.SdkHandler;

public sealed class Response
{
    public Response(int statusCode, string body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}
