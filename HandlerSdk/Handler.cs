using System.Collections;
using Yandex.Cloud.Functions;

namespace YandexCloud.Examples.CloudFunctions.SdkHandler;

public sealed class Handler : YcFunction<HttpRequest, Response>
{
    public Response FunctionHandler(HttpRequest req, Context context)
    {
        var vars = Environment.GetEnvironmentVariables();

        Console.WriteLine("Env vars:");
        foreach (DictionaryEntry entry  in vars)
        {
            Console.Write(entry.Key);
            Console.Write(" = ");
            Console.WriteLine(entry.Value);
        }


        Console.WriteLine("Request:");
        Console.WriteLine(req.HttpMethod);
        Console.WriteLine(req.Body);
        foreach (var header in req.Headers)
        {
            Console.Write(header.Key);
            Console.Write(": ");
            Console.WriteLine(header.Value);
        }

        Console.WriteLine("Context:");
        foreach (var property in context.GetType().GetProperties())
        {
            Console.Write(property.Name);
            Console.Write(" = ");
            Console.WriteLine(property.GetValue(context));
        }


        return new Response(200,  "Hello world");
    }
}


