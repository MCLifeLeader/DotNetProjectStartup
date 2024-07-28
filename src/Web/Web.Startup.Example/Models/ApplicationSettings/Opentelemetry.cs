using Startup.Web.Helpers.Data;

namespace Startup.Web.Models.ApplicationSettings
{
    public class Opentelemetry
    {
        public string Endpoint { get; set; }
        [SensitiveData]
        public string ApiKey { get; set; }
    }
}