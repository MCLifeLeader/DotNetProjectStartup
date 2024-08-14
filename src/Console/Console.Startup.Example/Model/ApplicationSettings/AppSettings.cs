using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Startup.Console.Helpers.Data;
using System.Xml.Serialization;

namespace Startup.Console.Model.ApplicationSettings;

public record AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
#pragma warning disable LOGGEN036
    public IConfiguration ConfigurationBase { get; set; } = default!;
#pragma warning restore LOGGEN036

    public Logging Logging { get; set; } = default!;
    public Opentelemetry OpenTelemetry { get; set; } = default!;
    public Featuremanagement FeatureManagement { get; set; } = default!;
    public string KeyVaultUri { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
    public Connectionstrings ConnectionStrings { get; set; } = default!;
    public Workerprocesses WorkerProcesses { get; set; } = default!;
    public Startupexample StartupExample { get; set; } = default!;
   
    [SensitiveData]
    public string RedactionKey { get; set; } = default!;
}