using Startup.Blazor.Server.Helpers.Data;

namespace Startup.Blazor.Server.Models.ApplicationSettings
{
    public class Opentelemetry
    {
        public string Endpoint { get; set; }
        [SensitiveData]
        public string ApiKey { get; set; }
    }
}