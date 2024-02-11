using Startup.Api.Models.ApplicationSettings;

namespace Startup.Api.Helpers.Extensions;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Application Settings")]
    public static partial void LogAppSettings(
        this ILogger logger,
       [LogProperties] AppSettings settings);
}