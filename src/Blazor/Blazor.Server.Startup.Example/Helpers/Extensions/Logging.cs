using Startup.Blazor.Server.Models.ApplicationSettings;

namespace Startup.Blazor.Server.Helpers.Extensions;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Application Settings")]
#pragma warning disable EXTEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static partial void LogAppSettings(this ILogger logger, [LogProperties(Transitive = true)] AppSettings settings);
#pragma warning restore EXTEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}