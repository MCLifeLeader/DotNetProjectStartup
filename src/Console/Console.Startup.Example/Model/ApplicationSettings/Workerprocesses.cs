using Microsoft.Extensions.Logging;

namespace Startup.Console.Model.ApplicationSettings;

public record Workerprocesses
{
    public int SleepDelaySeconds { get; set; }
    public Startupapi StartupApi { get; set; } = default!;
}