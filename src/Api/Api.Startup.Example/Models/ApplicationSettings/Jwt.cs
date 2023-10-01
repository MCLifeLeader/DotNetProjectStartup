namespace Api.Startup.Example.Models.ApplicationSettings
{
    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireInMinutes { get; set; }
        public string Subject { get; set; }
        public string Key { get; set; }
    }
}