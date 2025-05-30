using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Function.Api.Helpers.Health;

public class StartupExampleApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly ILogger<StartupExampleApiHealthCheck> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public StartupExampleApiHealthCheck(
        ILogger<StartupExampleApiHealthCheck> logger,
        IHttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClient = httpClientWrapper;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var data = await _httpClient.GetBytesAsync("_health", HttpClientNames.STARTUPEXAMPLE_API);

            if (data is { Length: > 0 })
            {
                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process health request: {Message}", ex.Message);

            return HealthCheckResult.Unhealthy(ex.Message);
        }

        return HealthCheckResult.Unhealthy();
    }
}