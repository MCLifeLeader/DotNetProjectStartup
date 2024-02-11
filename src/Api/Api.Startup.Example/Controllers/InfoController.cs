using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Startup.Api.Models.ApplicationSettings;
using Startup.Api.Services.Interfaces;
using Startup.Common.Constants;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Net;
using System.Text;

namespace Startup.Api.Controllers;

[Authorize]
[ApiController]
[FeatureGate("InformationEndpoints")]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly AppSettings _appSettings;
    private readonly IInfoService _infoService;
    private readonly ILogger<InfoController> _logger;
    private readonly IFeatureManager _featureManager;

    public InfoController(
        ILogger<InfoController> logger,
        IOptions<AppSettings> appSettings,
        IFeatureManager featureManager,
        IInfoService infoService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));

        _logger.LogDebug("{ControllerName} init.", nameof(InfoController));
    }

    /// <summary>
    /// Service Status in XML format
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("StatusXml")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns the page status", typeof(ContentResult))]
    public async Task<ContentResult> GetStatusInformationXml()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetStatusInformationXml));
        await Task.Yield();

        return Content(_infoService.SerializeToResponseXml(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// Service status in JSON format
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("StatusJson")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns the page status", typeof(ContentResult))]
    public async Task<ContentResult> GetStatusInformationJson()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetStatusInformationJson));
        await Task.Yield();

        return Content(_infoService.SerializeToResponseJson(), "application/json", Encoding.UTF8);
    }

    [Authorize(Roles = "AgencyAdmin")]
    [HttpGet("Settings")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns App Settings", typeof(ContentResult))]
    public async Task<ActionResult<AppSettings>> GetAppSettings()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetAppSettings));
        await Task.Yield();

        if (await _featureManager.IsEnabledAsync(FeatureFlags.DISPLAY_CONFIGURATION))
        {
            return Ok(_appSettings);
        }

        return Forbid();
    }

    [Authorize(Roles = "AgencyAdmin")]
    [HttpGet("Environment")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns App Settings", typeof(ContentResult))]
    public async Task<ActionResult<IDictionary>> GetEnvironment()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetEnvironment));

        await Task.Yield();

        if (_appSettings.FeatureManagement.DisplayConfiguration)
        {
            return Ok(Environment.GetEnvironmentVariables());
        }

        return Forbid();
    }
}