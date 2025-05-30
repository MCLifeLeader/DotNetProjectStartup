using Startup.Common.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings;

public record Jwt
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpireInMinutes { get; set; }
    public string Subject { get; set; } = default!;

    [SensitiveData]
    public string Key { get; set; } = default!;
}