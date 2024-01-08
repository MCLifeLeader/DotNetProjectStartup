using Api.Startup.Example.Models.ApplicationSettings;

namespace Api.Startup.Example.Helpers.Extensions;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Application Settings")]
    public static partial void LogAppSettings(
        this ILogger logger,
       [LogProperties] AppSettings settings);
}