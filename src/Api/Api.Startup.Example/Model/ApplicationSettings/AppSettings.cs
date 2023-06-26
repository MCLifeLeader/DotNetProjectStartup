namespace Api.Startup.Example.Model.ApplicationSettings;

public class AppSettings
{
    public IConfiguration ConfigurationBase { get; internal set; }

    public Logging Logging { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public string KeyVaultUri { get; set; }
    public bool DisplayConfiguration { get; set; }
    public string AllowedHosts { get; set; }
    public Jwt Jwt { get; set; }
    public bool CorsEnabled { get; set; }
    public bool SwaggerEnabled { get; set; }
    public int CacheDurationInSeconds { get; set; }
    public string PageUrl { get; set; }
}