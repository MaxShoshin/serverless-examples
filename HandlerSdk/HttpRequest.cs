using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.SdkHandler;

public sealed class HttpRequest
{
    [JsonPropertyName("httpMethod")]
    public string HttpMethod { get; set; } = string.Empty;

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("multiValueHeaders")]
    public Dictionary<string, List<string>> MultiValueHeaders { get; set; } = new();

    [JsonPropertyName("queryStringParameters")]
    public Dictionary<string, string> QueryStringParams { get; set; } = new();

    [JsonPropertyName("multiValueQueryStringParameters")]
    public Dictionary<string, List<string>> MultiValueQueryStringParams { get; set; } = new();

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("requestContext")]
    public RequestContext RequestContext { get; set; } = new();

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("isBase64Encoded")]
    public bool IsBase64Encoded { get; set; }
}
