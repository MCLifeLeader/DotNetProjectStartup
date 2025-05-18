using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Startup.Function.Api.Services;
using Startup.Function.Api.Services.Interfaces;
using System.Net;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Startup.Function.Api.Functions;

public class HealthCheckFunctions
{
    private readonly ILogger<HealthCheckFunctions> _logger;
    private readonly IHealthService _healthCheckService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HealthCheckFunctions(
        ILogger<HealthCheckFunctions> logger,
        IHealthService healthCheckService)
    {
        _logger = logger;
        _healthCheckService = healthCheckService;
    }

    #region HealthCheck
    [OpenApiOperation(
        "health",
        "Health Check",
        Summary = "Health Check",
        Description = "Health Check",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        "application/json",
        typeof(HealthService.HealthData),
        Summary = "HealthReport",
        Description = "HealthReport")]
    [Function("health")]
    public async Task<IActionResult> HealthCheck([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext context)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(Common.Constants.LoggingTemplates.DebugMethodEntryMessage, GetType().Name, nameof(HealthCheck));
        }

        return await _healthCheckService.CheckHealthAsync();
    }

    #endregion

    [OpenApiOperation(
        "information",
        "Application",
        Summary = "Information",
        Description = "Information",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        "application/json",
        typeof(string),
        Summary = "Information",
        Description = "Information")]
    [Function("information")]
    public async Task<IActionResult> AppInformation([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext context)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(Common.Constants.LoggingTemplates.DebugMethodEntryMessage, GetType().Name, nameof(HealthCheck));
        }

        return await Task.FromResult(new OkObjectResult(new AppInfo($"{GetType().Assembly.GetName().Version}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown")));
    }

    internal record AppInfo(string Version, string Environment);
}