using Microsoft.Extensions.Logging;
using Startup.Function.Api.Models.AppSettings;
using System.Diagnostics.CodeAnalysis;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Startup.Function.Api.Helpers.Extensions;

/// <summary>
/// Logging extension methods.
/// Add new methods for data that should be logged and has sensitive data contained within.
/// </summary>
[ExcludeFromCodeCoverage]
public static partial class Logging
{
#pragma warning disable EXTEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    [LoggerMessage(LogLevel.Information, "Application Settings")]
    public static partial void LogAppSettings(this ILogger logger, [LogProperties(Transitive = true)] AppSettings settings);

#pragma warning restore EXTEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}