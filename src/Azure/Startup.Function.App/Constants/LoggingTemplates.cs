using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Constants;

[ExcludeFromCodeCoverage]
public class LoggingTemplates : Common.Constants.LoggingTemplates
{
    public static readonly string ErrorHealthCheckMessage = "Health Check Failed: {Message}";
    public static readonly string InfoHealthStatus = "HealthStatus: {HealthStatus}";
    public static readonly string ApplicationError = "There was an Error: {Data}";
}