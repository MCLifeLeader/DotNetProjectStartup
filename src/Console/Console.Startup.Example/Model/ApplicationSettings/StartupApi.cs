using Startup.Console.Helpers.Data;

namespace Startup.Console.Model.ApplicationSettings;

public class Startupapi
{
    public string Uri { get; set; }
    public string Cron { get; set; }
    public int TimeOutInSeconds { get; set; }
    [PiiData]
    public string Username { get; set; }
    [SensitiveData]
    public string Password { get; set; }
}