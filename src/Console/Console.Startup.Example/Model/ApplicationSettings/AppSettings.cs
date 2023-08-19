using Microsoft.Extensions.Configuration;

namespace Console.Startup.Example.Model.ApplicationSettings;

public class AppSettings
{
    public IConfiguration ConfigurationBase { get; set; }

    public Featuremanagement FeatureManagement { get; set; }
    public Logging Logging { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public string KeyVaultUri { get; set; }
    public string ServiceName { get; set; }
    public Workerprocesses WorkerProcesses { get; set; }
    public Applicationinsights ApplicationInsights { get; set; }
}