using Newtonsoft.Json;
using Startup.Api.Helpers.Data;
using System.Xml.Serialization;

namespace Startup.Api.Models.ApplicationSettings;

public class AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
    public IConfiguration ConfigurationBase { get; internal set; }

    public Logging Logging { get; set; }
    public Opentelemetry OpenTelemetry { get; set; }
    public FeatureManagement FeatureManagement { get; set; }
    public string KeyVaultUri { get; set; }
    public int CacheDurationInSeconds { get; set; }

    [SensitiveData]
    public string RedactionKey { get; set; }

    public ConnectionStrings ConnectionStrings { get; set; }

    public HttpClients HttpClients { get; set; }
    public HealthCheckEndpoint HealthCheckEndpoints { get; set; }

    public StartupExample StartupExample { get; set; }
    public Jwt Jwt { get; set; }
    public string AllowedHosts { get; set; }
}