using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.Handlers;

/// <summary>
/// Урезанный пример запроса, HTTP вызов.
/// </summary>
public sealed class HttpRequest
{
    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}


/// <summary>
/// Класс с ответом.
/// </summary>
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
