using Console.Startup.Example.Helpers.Data;

namespace Console.Startup.Example.Model.ApplicationSettings;

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