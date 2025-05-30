namespace Startup.Blazor.Server.Models.ApplicationSettings;

public record Featuremanagement
{
    public bool OpenTelemetryEnabled { get; set; }
    public bool InformationEndpoints { get; set; }
    public bool SqlDebugger { get; set; }
    public bool DisplayConfiguration { get; set; }
}