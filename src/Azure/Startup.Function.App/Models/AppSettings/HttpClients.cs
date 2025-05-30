using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class HttpClients
{
    public Resilience? Resilience { get; set; }
    public StartupExample? StartupExample { get; set; }
}