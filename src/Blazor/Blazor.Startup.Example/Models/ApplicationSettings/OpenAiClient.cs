using Startup.Blazor.Server.Helpers.Data;

namespace Startup.Blazor.Server.Models.ApplicationSettings
{
    public class OpenAiClient
    {
        public string BaseUrl { get; set; }

        [SensitiveData]
        public string ApiKey { get; set; }
        public string AiModel { get; set; }
        public int TimeoutInSeconds { get; set; }
        public int CacheDurationInSeconds { get; set; }
    }
}