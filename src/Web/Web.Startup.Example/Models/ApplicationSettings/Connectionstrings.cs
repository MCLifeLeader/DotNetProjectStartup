using Web.Startup.Example.Helpers.Data;

namespace Web.Startup.Example.Models.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}