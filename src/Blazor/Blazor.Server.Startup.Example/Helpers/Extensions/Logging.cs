using Startup.Blazor.Server.Models.ApplicationSettings;

namespace Startup.Blazor.Server.Helpers.Extensions;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Application Settings")]
    public static partial void LogAppSettings(this ILogger logger, [LogProperties] AppSettings settings);
}