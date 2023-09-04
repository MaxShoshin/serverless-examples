using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class MessageItem
{
    [JsonPropertyName("event_metadata")]
    public Metadata Metadata { get; set; } = new();

    [JsonPropertyName("details")]
    public Details Details { get; set; } = new Details();
}
