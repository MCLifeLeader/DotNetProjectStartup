using Api.Startup.Example.Helpers.Data;

namespace Api.Startup.Example.Models.ApplicationSettings;

public class Connectionstrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; }
    [SensitiveData]
    public string AzuriteBlobStorage { get; set; }
    [SensitiveData]
    public string ServiceBus { get; set; }
    public string ServiceBusQueue { get; set; }
    [SensitiveData]
    public string ApplicationInsights { get; set; }
}