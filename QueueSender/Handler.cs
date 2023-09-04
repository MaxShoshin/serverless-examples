using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace YandexCloud.Examples.CloudFunctions.QueueSender;

public sealed class Handler
{
    private static readonly Lazy<AmazonSQSClient> s_sqsClient = new (CreateClient());

    public async Task<string> FunctionHandler(JsonElement message)
    {
        var res = await SendMessageAsync(
            messageBody: JsonSerializer.Serialize(message),
            messageAttributes: new());

        return res.MessageId;
    }

    private static async Task<SendMessageResponse> SendMessageAsync(
        string messageBody,
        Dictionary<string, MessageAttributeValue> messageAttributes)
    {
        var queueUrl = GetEnvironmentVariable("QUEUE_URL");

        Console.WriteLine("Sending message to " + queueUrl);

        var sendMessageRequest = new SendMessageRequest
        {
            DelaySeconds = 10,
            MessageAttributes = messageAttributes,
            MessageBody = messageBody,
            QueueUrl = queueUrl,
        };

        var response = await s_sqsClient.Value.SendMessageAsync(sendMessageRequest);

        Console.WriteLine($"Sent a message with id : {response.MessageId}");

        return response;
    }

    private static AmazonSQSClient CreateClient()
    {
        // Validate environment variable existence,
        // They values are used to authentication in Amazon SQS client
        GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

        var cfg = new AmazonSQSConfig
        {
            AuthenticationRegion = "ru-central1",
            ServiceURL = "https://message-queue.api.cloud.yandex.net/"
        };

        return new AmazonSQSClient(cfg);
    }

    private static string GetEnvironmentVariable(string name) =>
        Environment.GetEnvironmentVariable(name) ??
        throw new InvalidOperationException($"Missing «{name}» environment variable");
}
