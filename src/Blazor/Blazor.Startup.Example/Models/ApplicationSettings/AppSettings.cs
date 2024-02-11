using Startup.Blazor.Server.Helpers.Data;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Startup.Blazor.Server.Models.ApplicationSettings;

public class AppSettings
{
    [JsonIgnore]
    [XmlIgnore]
    public IConfiguration ConfigurationBase { get; set; }

    public Featuremanagement FeatureManagement { get; set; }
    public Logging Logging { get; set; }
    public string KeyVaultUri { get; set; }
    public bool DisplayConfiguration { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public Startupexample StartupExample { get; set; }
    public string AllowedHosts { get; set; }
    public object StorageAccount { get; internal set; }
    [SensitiveData]
    public string RedactionKey { get; set; }
}