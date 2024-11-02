using Startup.Common.Helpers.Data;

namespace Startup.Blazor.Server.Models.ApplicationSettings;

public record Opentelemetry
{
    public string Endpoint { get; set; } = default!;

    [SensitiveData]
    public string ApiKey { get; set; } = default!;
}