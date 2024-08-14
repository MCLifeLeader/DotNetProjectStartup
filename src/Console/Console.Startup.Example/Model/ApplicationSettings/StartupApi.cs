using Startup.Console.Helpers.Data;

namespace Startup.Console.Model.ApplicationSettings;

public record Startupapi
{
    public string Uri { get; set; } = default!;
    public string Cron { get; set; } = default!;
    public int TimeOutInSeconds { get; set; }

    [PiiData]
    public string Username { get; set; } = default!;
 
    [SensitiveData]
    public string Password { get; set; } = default!;
}