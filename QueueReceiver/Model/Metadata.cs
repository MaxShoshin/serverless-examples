using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class Metadata
{
    [JsonPropertyName("event_id")]
    public string EventId { get; set; } = string.Empty;

    [JsonPropertyName("event_type")]
    public string EventType { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("cloud_id")]
    public string CloudId { get; set; } = string.Empty;

    [JsonPropertyName("folder_id")]
    public string FolderId { get; set; } = string.Empty;

    // [JsonPropertyName("tracing_context")]
    // public Dictionary<string,object> TracingContext { get; } = new Dictionary<string, object>()
}
