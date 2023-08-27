using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class MessageRequest
{
    [JsonPropertyName("messages")]
    public List<MessageItem> Messages { get; set; } = new();
}
