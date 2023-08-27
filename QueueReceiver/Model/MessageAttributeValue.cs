using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.QueueReceiver.Model;

public sealed class MessageAttributeValue
{
    [JsonPropertyName("dataType")]
    public string DataType { get; set; } = string.Empty; // Can be Enum

    [JsonPropertyName("stringValue")]
    public string StringValue { get; set; } = string.Empty;
}
