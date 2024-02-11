using Startup.Api.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings;

public class ConnectionStrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }

    [SensitiveData]
    public string ServiceBus { get; set; }
    public string ServiceBusQueue { get; set; }

    [SensitiveData]
    public string ApplicationInsights { get; set; }
}