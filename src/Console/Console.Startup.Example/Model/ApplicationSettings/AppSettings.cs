using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Startup.Console.Helpers.Data;
using System.Xml.Serialization;

namespace Startup.Console.Model.ApplicationSettings;

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