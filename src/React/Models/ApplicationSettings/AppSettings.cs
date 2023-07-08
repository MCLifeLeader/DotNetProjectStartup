namespace React.Startup.Example.Models.ApplicationSettings;

public class AppSettings
{
    public IConfiguration ConfigurationBase { get; internal set; }

    public Logging Logging { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public Identityserver IdentityServer { get; set; }
    public string KeyVaultUri { get; set; }
    public bool DisplayConfiguration { get; set; }
    public string AllowedHosts { get; set; }
    public string PageUrl { get; set; }
    public bool SwaggerEnabled { get; set; }
    public int CacheDurationInSeconds { get; set; }
}