using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class FeatureManagement
{
    public bool OpenApiEnabled { get; set; }
    public bool OpenTelemetryEnabled { get; set; }
    public bool SqlDebugger { get; set; }
}