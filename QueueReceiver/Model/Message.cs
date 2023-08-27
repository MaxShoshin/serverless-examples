using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class Message
{
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; } = string.Empty;

    [JsonPropertyName("md5_of_body")]
    public string BodyMD5 { get; set; } = string.Empty;

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("attributes")]
    public Dictionary<string,string> Attributes { get; set; } = new(); // Standard attributes, can deserialized directly to custom type

    [JsonPropertyName("message_attributes")]
    public Dictionary<string, MessageAttributeValue> CustomAttributes { get; set; } = new();

    [JsonPropertyName("md5_of_message_attributes")]
    public string MessageAttributeMD5 { get; set; } = string.Empty;
}
