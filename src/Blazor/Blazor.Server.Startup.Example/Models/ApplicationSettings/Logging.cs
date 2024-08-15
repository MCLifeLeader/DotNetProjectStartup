namespace Startup.Blazor.Server.Models.ApplicationSettings;

public record Logging
{
    public Loglevel LogLevel { get; set; } = default!;
}