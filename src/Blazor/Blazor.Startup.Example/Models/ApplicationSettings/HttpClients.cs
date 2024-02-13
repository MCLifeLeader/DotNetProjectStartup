namespace Startup.Blazor.Server.Models.ApplicationSettings
{
    public class HttpClients
    {
        public OpenAiClient OpenAi { get; set; }
        public OpenAiClient AzureOpenAi { get; set; }
    }
}