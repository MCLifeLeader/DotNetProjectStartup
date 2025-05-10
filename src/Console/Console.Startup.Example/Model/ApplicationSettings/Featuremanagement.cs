namespace Startup.Console.Model.ApplicationSettings;

public record Featuremanagement
{
    public bool OpenTelemetryEnabled { get; set; }
    public bool HealthCheckWorker { get; set; }
    public bool FileWorker { get; set; }
    public bool SqlDebugger { get; set; }
}