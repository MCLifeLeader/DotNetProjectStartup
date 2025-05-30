namespace Startup.Api.Models.ApplicationSettings;

public record Logging
{
    public Loglevel LogLevel { get; set; } = default!;
}