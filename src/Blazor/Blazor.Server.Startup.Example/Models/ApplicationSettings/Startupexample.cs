namespace Startup.Blazor.Server.Models.ApplicationSettings;

public record Startupexample
{
    public string ApiUrl { get; set; } = default!;
    public string AppUrl { get; set; } = default!;
    public string WebUrl { get; set; } = default!;
    public string TempMediaPath { get; set; } = default!;
}