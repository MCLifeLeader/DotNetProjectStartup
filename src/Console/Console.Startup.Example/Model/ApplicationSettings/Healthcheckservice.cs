namespace Console.Startup.Example.Model.ApplicationSettings
{
    public class Healthcheckservice
    {
        public string Uri { get; set; }
        public string Cron { get; set; }
        public int TimeOutInSeconds { get; set; }
    }
}