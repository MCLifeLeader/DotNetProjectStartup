using Startup.Web.Helpers.Data;

namespace Startup.Web.Models.ApplicationSettings
{
    public record Opentelemetry
    {
        public string Endpoint { get; set; } = default!;

        [SensitiveData]
        public string ApiKey { get; set; } = default!;
    }
}