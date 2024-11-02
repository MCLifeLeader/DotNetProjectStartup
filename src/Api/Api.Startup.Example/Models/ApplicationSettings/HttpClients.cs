namespace Startup.Api.Models.ApplicationSettings;

public record HttpClients
{
    public OpenAiClient OpenAi { get; set; } = default!;
    public OpenAiClient AzureOpenAi { get; set; } = default!;
    public Resilience Resilience { get; set; } = default!;
}