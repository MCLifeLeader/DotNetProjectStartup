using Startup.Common.Helpers.Data;

namespace Startup.Console.Model.ApplicationSettings;

public record Opentelemetry
{
    public string Endpoint { get; set; } = default!;

    [SensitiveData]
    public string ApiKey { get; set; } = default!;
}