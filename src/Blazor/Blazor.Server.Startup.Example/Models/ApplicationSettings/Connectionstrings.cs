using Startup.Blazor.Server.Helpers.Data;

namespace Startup.Blazor.Server.Models.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}