using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Api.Helpers.Health;

public class OpenAiHealthCheck : IHealthCheck
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly ILogger<OpenAiHealthCheck> _logger;

    public OpenAiHealthCheck(ILogger<OpenAiHealthCheck> logger,
        IHttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClient = httpClientWrapper;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            OpenAiHealthStatus data = await _httpClient.GetObjectAsync<OpenAiHealthStatus>("api/v2/status.json", HttpClientNames.OPEN_AI_API_HEALTH);

            if (data.Page is { Name: not null } && data.Page.Name.Contains("OpenAI"))
            {
                if (data.Status is { Indicator: not null } && data.Status.Indicator.ToLower().Contains("none"))
                {
                    _logger.LogInformation($"Health Check: {data.Status.Description}");
                    return HealthCheckResult.Healthy(data.Status.Description);
                }

                if (data.Status is { Indicator: not null } && data.Status.Indicator.ToLower().Contains("minor"))
                {
                    _logger.LogWarning($"Health Check: {data.Status.Description}");
                    return HealthCheckResult.Degraded(data.Status.Description);
                }

                // This is an assumption don't know yet
                if (data.Status is { Indicator: not null } && data.Status.Indicator.ToLower().Contains("major"))
                {
                    _logger.LogError($"Health Check: {data.Status.Description}");
                    return HealthCheckResult.Unhealthy(data.Status.Description);
                }
            }

            // Fallback health status
            _logger.LogError($"Health Check Fallback: {data.Status?.Description}");
            return HealthCheckResult.Unhealthy(data.Status?.Description);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Health Check: {ex.Message}");
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }

    #region Open AI Health Check Status Object
    public class OpenAiHealthStatus
    {
        [JsonProperty("page")]
        public Page? Page { get; set; }

        [JsonProperty("status")]
        public Status? Status { get; set; }
    }

    public class Page
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("time_zone")]
        public string? TimeZone { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class Status
    {
        [JsonProperty("indicator")]
        public string? Indicator { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
    #endregion
}
