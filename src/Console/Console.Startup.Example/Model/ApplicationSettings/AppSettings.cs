using Console.Startup.Example.Helpers.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Console.Startup.Example.Model.ApplicationSettings;

public class AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
    public IConfiguration ConfigurationBase { get; set; }

    public Featuremanagement FeatureManagement { get; set; }
    public Logging Logging { get; set; }
    public string KeyVaultUri { get; set; }
    public string ServiceName { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public Workerprocesses WorkerProcesses { get; set; }
    public Startupexample StartupExample { get; set; }
    [SensitiveData]
    public string RedactionKey { get; set; }
}