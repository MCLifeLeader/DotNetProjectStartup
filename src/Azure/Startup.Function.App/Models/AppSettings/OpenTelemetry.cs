using Startup.Common.Helpers.Data;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class Opentelemetry
{
    public string? Endpoint { get; set; }

    [SensitiveData]
    public string? ApiKey { get; set; }
}