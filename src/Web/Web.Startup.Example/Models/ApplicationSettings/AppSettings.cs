using Newtonsoft.Json;
using System.Xml.Serialization;
using Startup.Web.Helpers.Data;

namespace Startup.Web.Models.ApplicationSettings;

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

    [SensitiveData]
    public string RedactionKey { get; set; } = default!;

    public string KeyVaultUri { get; set; } = default!;
    public Connectionstrings ConnectionStrings { get; set; } = default!;
    public Startupexample StartupExample { get; set; } = default!;
    public HealthCheckEndpoint HealthCheckEndpoints { get; set; } = default!;
    public HttpClients HttpClients { get; set; } = default!;

    public string AllowedHosts { get; set; } = default!;
}