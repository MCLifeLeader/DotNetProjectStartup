using Startup.Console.Helpers.Data;

namespace Startup.Console.Model.ApplicationSettings
{
    public class Opentelemetry
    {
        public string Endpoint { get; set; }
        [SensitiveData]
        public string ApiKey { get; set; }
    }
}