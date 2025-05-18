using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Startup.Function.Api.Constants;
using Startup.Function.Api.Services.Interfaces;
using System.Text.Json.Serialization;

namespace Startup.Function.Api.Services;

public class HealthService : IHealthService
{
    private readonly ILogger<HealthService> _logger;
    private readonly HealthCheckService _healthCheckService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HealthService(
        ILogger<HealthService> logger,
        HealthCheckService healthCheckService)
    {
        _logger = logger;
        _healthCheckService = healthCheckService;
    }

    public async Task<ObjectResult> CheckHealthAsync()
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(Common.Constants.LoggingTemplates.DebugMethodEntryMessage, GetType().Name, nameof(CheckHealthAsync));
        }

        try
        {
            // The response from the health check should always be 200 as the health check itself returned successfully.
            // See inner details for the actual health status.
            var report = await _healthCheckService.CheckHealthAsync();

            var healthData = new HealthData
            {
                Status = report.Status.ToString(),
                HealthReport = new HealthReportData()
                {
                    Status = report.Status,
                    TotalDuration = report.TotalDuration,
                    Entries = report.Entries.Select(entry => new Entry
                    {
                        Key = entry.Key,
                        Data = entry.Value.Data,
                        Description = entry.Value.Description ?? string.Empty,
                        Duration = entry.Value.Duration.ToString(),
                        Status = (int)entry.Value.Status,
                        Tags = entry.Value.Tags
                    }).ToList()
                }
            };

            return await Task.FromResult(new OkObjectResult(healthData));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoggingTemplates.ErrorHealthCheckMessage, ex.Message);
        }

        return await Task.FromResult(new OkObjectResult("Unhealthy"));
    }

    internal class HealthData
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        public required HealthReportData HealthReport { get; set; }
    }

    internal class HealthReportData
    {
        public IList<Entry>? Entries { get; set; }
        public HealthStatus Status { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }

    internal class Entry
    {
        public object? Data { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public int Status { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public string? Key { get; set; }
    }
}
