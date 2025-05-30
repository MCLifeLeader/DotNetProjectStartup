using Microsoft.Extensions.Configuration;
using Startup.Common.Helpers.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
    public IConfiguration? ConfigurationBase { get; set; }

    public Logging? Logging { get; set; }

    [SensitiveData]
    public string? RedactionKey { get; set; }
    public int DefaultPagingSize { get; set; }

    public FeatureManagement? FeatureManagement { get; set; }
    public ConnectionStrings? ConnectionStrings { get; set; }
    public Opentelemetry? OpenTelemetry { get; set; }

    public HttpClients? HttpClients { get; set; }
}
