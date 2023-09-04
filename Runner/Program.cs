using YandexCloud.Examples.CloudFunctions.QrCoder;

//var response = await new Handler().FunctionHandler(JsonDocument.Parse("{}").RootElement);

//var response = new BenchHandler().FunctionHandler("");
//Console.WriteLine(response.Body);

Console.WriteLine(await new Handler().FunctionHandler(new HttpRequest()));
