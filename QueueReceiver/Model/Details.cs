using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class Details
{
    [JsonPropertyName("queue_id")]
    public string QueueId { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public Message Message { get; set; } = new Message();
}
