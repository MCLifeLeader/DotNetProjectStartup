namespace Startup.Api.Models.ApplicationSettings;

public record FeatureManagement
{
    public bool OpenTelemetryEnabled { get; set; }
    public bool InformationEndpoints { get; set; }
    public bool SqlDebugger { get; set; }
    public bool DisplayConfiguration { get; set; }
    public bool CorsEnabled { get; set; }
    public bool SwaggerEnabled { get; set; }
}