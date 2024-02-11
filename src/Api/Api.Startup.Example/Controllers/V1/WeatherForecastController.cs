using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startup.Api.Models;

namespace Startup.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : BaseController
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("GetWeatherForecast1")]
    public IEnumerable<WeatherForecast> Get1()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Get1));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [Authorize]
    [HttpGet("GetWeatherForecast2")]
    public IEnumerable<WeatherForecast> Get2()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Get2));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [Authorize(Roles = "AgencyAdmin")]
    [HttpGet("GetWeatherForecast4")]
    public IEnumerable<WeatherForecast> Get3()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Get3));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [Authorize(Roles = "OtherRole")]
    [HttpGet("GetWeatherForecast3")]
    public IEnumerable<WeatherForecast> Get4()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Get4));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}