using Newtonsoft.Json;
using System.Xml.Serialization;
using Startup.Web.Helpers.Data;

namespace Startup.Web.Models.ApplicationSettings;

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
    [SensitiveData]
    public string RedactionKey { get; set; }
}