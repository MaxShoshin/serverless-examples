using System.Reflection;
using BenchmarkDotNet.Environments;
using Hardware.Info;

namespace YandexCloud.Examples.CloudFunctions.Info;

partial class View
{
    public View(string request, HardwareInfo hardwareInfo, BenchmarkEnvironmentInfo envInfo, string diskInfo, string logs, string releases)
    {
        Request = request;
        HardwareInfo = hardwareInfo;
        EnvInfo = envInfo;

        DiskInfo = diskInfo;
        Logs = logs;
        Releases = releases;
    }

    private BenchmarkEnvironmentInfo EnvInfo { get; }

    private string Request { get; }

    private string DiskInfo { get; }

    private string Logs { get; }

    public string Releases { get; }

    private HardwareInfo HardwareInfo { get; }

    private IEnumerable<(string Name, string Value)> GetEnvironmentProperties()
    {
        var properties = typeof(Environment).GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var property in properties)
        {
            var value = property.GetValue(null)?.ToString();
            if  (value != null)
            {
                yield return new(property.Name, value);
            }
        }
    }
}
