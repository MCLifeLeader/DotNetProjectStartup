using Microsoft.Extensions.Logging;

namespace Startup.Console.Model.ApplicationSettings;

public record Logging
{
    public Loglevel LogLevel { get; set; } = default!;
}