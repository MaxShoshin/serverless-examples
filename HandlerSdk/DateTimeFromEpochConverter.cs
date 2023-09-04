using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexCloud.Examples.CloudFunctions.SdkHandler;

public sealed class DateTimeFromEpochConverter: JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var epoch = reader.GetInt64();
        return DateTime.UnixEpoch + TimeSpan.FromSeconds(epoch);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
