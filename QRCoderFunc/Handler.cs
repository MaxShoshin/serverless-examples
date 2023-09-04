using QRCoder;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bots.Json;
using Telegram.Bots.Types;

namespace YandexCloud.Examples.CloudFunctions.QrCoder;

public sealed class Handler
{
    private static readonly string s_botToken = GetEnvironmentVariable("BOT_TOKEN");
    private static readonly string s_apiKey = GetEnvironmentVariable("API_KEY");

    private static readonly TelegramBotClient s_client = new(s_botToken);
    private static readonly Serializer s_serializer = new Serializer();

    private static readonly Response s_ok = new Response(200, "Ok");
    private static readonly Response s_bad = new Response(400, "Bad");

    public async Task<Response> FunctionHandler(HttpRequest request)
    {
        // Validate sender (only telegram should know secret token, specified during WebHook set)
        if (!request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var apiKey) ||
            apiKey != s_apiKey)
        {
            return s_bad;
        }

        var msg = s_serializer.Deserialize<MessageUpdate>(request.Body);
        var chatId = new ChatId(msg.Data.Chat.Id);

        if (msg.Data is not TextMessage txtMsg)
        {
            await s_client.SendTextMessageAsync(chatId, "Не смог обработать ваш запрос");

            // Send OK. otherwise Telegram will retry
            return s_ok;
        }

        using var qrGenerator = new QRCodeGenerator();
        using var data = qrGenerator.CreateQrCode(txtMsg.Text, QRCodeGenerator.ECCLevel.Q);

        using var code = new PngByteQRCode(data);
        using var stream = new MemoryStream(code.GetGraphic(20));

        await s_client.SendPhotoAsync(chatId, new InputFileStream(stream));

        return s_ok;
    }

    private static string GetEnvironmentVariable(string name) =>
        Environment.GetEnvironmentVariable(name) ??
        throw new InvalidOperationException($"Missing «{name}» environment variable");
}
