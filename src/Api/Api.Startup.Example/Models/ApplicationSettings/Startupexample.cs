namespace Startup.Api.Models.ApplicationSettings;

public record StartupExample
{
    public string ApiUrl { get; set; } = default!;
    public string AppUrl { get; set; } = default!;
    public string WebUrl { get; set; } = default!;
    public string TempMediaPath { get; set; } = default!;
}