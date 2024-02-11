using Startup.Api.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings;

public class Jwt
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireInMinutes { get; set; }
    public string Subject { get; set; }
    [SensitiveData]
    public string Key { get; set; }
}