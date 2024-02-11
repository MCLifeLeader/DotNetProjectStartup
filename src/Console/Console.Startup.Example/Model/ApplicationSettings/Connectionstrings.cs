using Startup.Console.Helpers.Data;

namespace Startup.Console.Model.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}