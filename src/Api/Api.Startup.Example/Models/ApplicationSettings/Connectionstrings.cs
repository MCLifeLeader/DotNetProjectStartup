using Startup.Common.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings;

public record ConnectionStrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; } = default!;

    [SensitiveData]
    public string ServiceBus { get; set; } = default!;
    public string ServiceBusQueue { get; set; } = default!;

    [SensitiveData]
    public string ApplicationInsights { get; set; } = default!;
}