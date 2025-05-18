using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class LogLevel
{
    public string? Default { get; set; }
    public string? Microsoft { get; set; }
    public string? System { get; set; }
    public string? MicrosoftHostingLifetime { get; set; }
}