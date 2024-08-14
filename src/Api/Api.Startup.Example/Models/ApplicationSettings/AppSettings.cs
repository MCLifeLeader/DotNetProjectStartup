using Newtonsoft.Json;
using Startup.Api.Helpers.Data;
using System.Xml.Serialization;

namespace Startup.Api.Models.ApplicationSettings;

public record AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
#pragma warning disable LOGGEN036
    public IConfiguration ConfigurationBase { get; internal set; } = default!;
#pragma warning restore LOGGEN036

    public Logging Logging { get; set; } = default!;
    public Opentelemetry OpenTelemetry { get; set; } = default!;
    public FeatureManagement FeatureManagement { get; set; } = default!;
    public string KeyVaultUri { get; set; } = default!;
    public int CacheDurationInSeconds { get; set; }

    [SensitiveData]
    public string RedactionKey { get; set; } = default!;

    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    public HttpClients HttpClients { get; set; } = default!;
    public HealthCheckEndpoint HealthCheckEndpoints { get; set; } = default!;
    public StartupExample StartupExample { get; set; } = default!;
    public Jwt Jwt { get; set; } = default!;
    public string AllowedHosts { get; set; } = default!;
}