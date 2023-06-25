using Microsoft.Extensions.Configuration;

namespace Console.Startup.Example.Model;

public class AppSettings
{
    public IConfiguration ConfigurationBase { get; set; }

    public Logging Logging { get; set; }
    public string KeyVaultUri { get; set; }
    public string ServiceName { get; set; }
    public int WorkerRunTimeMinutes { get; set; }
    public int ServiceRunTimeSleepDelaySeconds { get; set; }
    public Dataconnection DataConnection { get; set; }
    public Applicationinsights ApplicationInsights { get; set; }
}