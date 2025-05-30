namespace Startup.Api.Models.ApplicationSettings;

public record Loglevel
{
    public string Default { get; set; } = default!;
    public string Microsoft { get; set; } = default!;
    public string MicrosoftAspNetCore { get; set; } = default!;
    public string System { get; set; } = default!;
}