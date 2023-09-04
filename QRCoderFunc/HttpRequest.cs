using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QrCoder;

public sealed class HttpRequest
{
    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}
