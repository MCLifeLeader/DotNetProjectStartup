using Microsoft.Extensions.Diagnostics.HealthChecks;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Web.Helpers.Health;

public class StartupExampleAppHealthCheck : IHealthCheck
{
    private readonly IHttpClientWrapper _httpClient;

    public StartupExampleAppHealthCheck(IHttpClientWrapper httpClientWrapper)
    {
        _httpClient = httpClientWrapper;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var data = await _httpClient.GetBytesAsync("_health", HttpClientNames.STARTUPEXAMPLE_API);

            if (data is {Length: > 0})
            {
                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }

        return HealthCheckResult.Unhealthy();
    }
}