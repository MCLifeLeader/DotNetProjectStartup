using Startup.Web.Helpers.Data;

namespace Startup.Web.Models.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}