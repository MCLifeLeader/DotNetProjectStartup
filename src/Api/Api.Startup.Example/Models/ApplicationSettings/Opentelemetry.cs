using Startup.Api.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings
{
    public class Opentelemetry
    {
        public string Endpoint { get; set; }
        [SensitiveData]
        public string ApiKey { get; set; }
    }
}