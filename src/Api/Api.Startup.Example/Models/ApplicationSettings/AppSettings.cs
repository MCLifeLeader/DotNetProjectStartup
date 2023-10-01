using Startup.Business.Models.ApplicationSettings;

namespace Api.Startup.Example.Models.ApplicationSettings;

public class AppSettings
{
    public IConfiguration ConfigurationBase { get; internal set; }

    public Featuremanagement FeatureManagement { get; set; }
    public Logging Logging { get; set; }
    public string KeyVaultUri { get; set; }
    public bool DisplayConfiguration { get; set; }
    public bool CorsEnabled { get; set; }
    public bool SwaggerEnabled { get; set; }
    public int CacheDurationInSeconds { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public StorageAccount StorageAccount { get; set; }
    public Startupexample StartupExample { get; set; }
    public Jwt Jwt { get; set; }
    public string AllowedHosts { get; set; }
}