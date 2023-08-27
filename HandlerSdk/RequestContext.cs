using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.SdkHandler;

public class RequestContext
{
    [JsonPropertyName("httpMethod")]
    public string HttpMethod { get; set; } = string.Empty;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;

    [JsonPropertyName("requestTime")]
    public DateTimeOffset RequestTime { get; set; }

    [JsonPropertyName("requestTimeEpoch")]
    [JsonConverter(typeof(DateTimeFromEpochConverter))]
    public DateTime RequestTimeEpoch { get; set; }
}
