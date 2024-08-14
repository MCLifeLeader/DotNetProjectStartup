using Microsoft.Extensions.Logging;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console.Helpers.Extensions;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Application Settings")]
    public static partial void LogAppSettings(this ILogger logger, [LogProperties] AppSettings settings);
}