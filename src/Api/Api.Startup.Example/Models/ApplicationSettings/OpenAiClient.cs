using Startup.Common.Helpers.Data;

namespace Startup.Api.Models.ApplicationSettings;

public record OpenAiClient
{
    public string BaseUrl { get; set; } = default!;

    [SensitiveData]
    public string ApiKey { get; set; } = default!;

    public string AiModel { get; set; } = default!;
    public int TimeoutInSeconds { get; set; }
    public int CacheDurationInSeconds { get; set; }
}