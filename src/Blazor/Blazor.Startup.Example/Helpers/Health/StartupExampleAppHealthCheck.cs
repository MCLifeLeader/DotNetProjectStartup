﻿using Blazor.Startup.Example.Connection.Interfaces;
using Blazor.Startup.Example.Constants;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor.Startup.Example.Helpers.Health;

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
            var data = await _httpClient.GetBytesAsync("", HttpClientNames.StartupExample_App);

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