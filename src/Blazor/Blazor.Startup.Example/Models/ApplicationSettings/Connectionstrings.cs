using Blazor.Startup.Example.Helpers.Data;

namespace Blazor.Startup.Example.Models.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}