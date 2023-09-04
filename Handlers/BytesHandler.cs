using System.Text;

namespace YandexCloud.Examples.CloudFunctions.Handlers;

public sealed class BytesHandler
{
    public byte[] FunctionHandler(byte[] body)
    {
        return Encoding.UTF8.GetBytes("OK");
    }
}
