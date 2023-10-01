namespace Api.Startup.Example.Models.ApplicationSettings
{
    public class Connectionstrings
    {
        public string DefaultConnection { get; set; }
        public string ServiceBus { get; set; }
        public string ServiceBusQueue { get; set; }
        public string ApplicationInsights { get; set; }
    }
}