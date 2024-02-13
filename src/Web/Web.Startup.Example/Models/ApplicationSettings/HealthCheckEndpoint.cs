namespace Startup.Web.Models.ApplicationSettings
{
    public class HealthCheckEndpoint
    {
        public string OpenAi { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}