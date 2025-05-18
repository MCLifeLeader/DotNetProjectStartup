using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class Logging
{
    public LogLevel? LogLevel { get; set; }
}