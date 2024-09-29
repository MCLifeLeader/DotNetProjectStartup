namespace Startup.Api.Models.ApplicationSettings;

public record HealthCheckEndpoint
{
    public string OpenAi { get; set; } = default!;
    public int TimeoutInSeconds { get; set; }
}