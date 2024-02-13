using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Web.Helpers.Health;

public class OpenAiHealthCheck : IHealthCheck
{
    private readonly IHttpClientWrapper _httpClient;

    public OpenAiHealthCheck(IHttpClientWrapper httpClientWrapper)
    {
        _httpClient = httpClientWrapper;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            OpenAiHealthStatus data = await _httpClient.GetObjectAsync<OpenAiHealthStatus>("api/v2/status.json", HttpClientNames.OPEN_AI_API_HEALTH);

            if (data.Page.Name.Contains("OpenAI") &&
                data.Status.Indicator.Contains("none") &&
                data.Status.Description.Contains("All Systems Operational"))
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Degraded();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }

    #region Open AI Health Check Status Object
    public class OpenAiHealthStatus
    {
        [JsonProperty("page")]
        public Page Page { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public class Page
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class Status
    {
        [JsonProperty("indicator")]
        public string Indicator { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    #endregion
}
