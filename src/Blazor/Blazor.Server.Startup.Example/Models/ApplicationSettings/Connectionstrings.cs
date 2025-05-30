using Startup.Common.Helpers.Data;

namespace Startup.Blazor.Server.Models.ApplicationSettings;

public record Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; } = default!;

    [SensitiveData]
    public string ApplicationInsights { get; set; } = default!;
}