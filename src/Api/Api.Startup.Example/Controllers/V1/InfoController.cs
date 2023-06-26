using System.Collections;
using System.Net;
using System.Text;
using Api.Startup.Example.Model.ApplicationSettings;
using Api.Startup.Example.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Startup.Example.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class InfoController : ControllerBase
{
    private readonly AppSettings _appSettings;
    private readonly IInfoService _canaryService;
    private readonly ILogger<InfoController> _logger;

    public InfoController(
        ILogger<InfoController> logger,
        IOptions<AppSettings> appSettings,
        IInfoService canaryService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _canaryService = canaryService ?? throw new ArgumentNullException(nameof(canaryService));

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

        return Content(_canaryService.SerializeToResponseXml(), "application/xml", Encoding.UTF8);
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

        return Content(_canaryService.SerializeToResponseJson(), "application/json", Encoding.UTF8);
    }

    [Authorize(Roles = "AgencyAdmin")]
    [HttpGet("Settings")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns App Settings", typeof(ContentResult))]
    public async Task<ActionResult<AppSettings>> GetAppSettings()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetAppSettings));

        await Task.Yield();

        if (_appSettings.DisplayConfiguration)
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

        if (_appSettings.DisplayConfiguration)
        {
            return Ok(Environment.GetEnvironmentVariables());
        }

        return Forbid();
    }
}