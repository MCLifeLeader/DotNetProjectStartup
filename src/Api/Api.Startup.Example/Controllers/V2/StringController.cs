using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Startup.Api.Controllers.V2;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("[controller]")] 
[Route("api/v{version:apiVersion}/[controller]")]
public class StringController : BaseController
{
    private readonly ILogger<StringController> _logger;
    private readonly Random _rand = new Random((int)DateTime.UtcNow.Ticks);
    public static string AlphabetNumbersSpecial { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+{}|[]]\\\"\':;,./<>?`~-=";

    // ReSharper disable once ConvertToPrimaryConstructor
    public StringController(ILogger<StringController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Generate random strings
    /// </summary>
    /// <param name="length">Length of the string to create</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetSomeRandomString/{length}")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, "A string of characters", typeof(string))]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "No data returned")]
    public async Task<ActionResult> GenerateSomeRandomString(int length)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GenerateSomeRandomString));

        if (length < 0)
        {
            return new BadRequestObjectResult("Length must be >= 0");
        }

        if (length == 0)
        {
            return new NoContentResult();
        }

        await Task.Yield();

        string randomString = new string(Enumerable.Repeat(AlphabetNumbersSpecial, length).Select(s => s[_rand.Next(s.Length)]).ToArray());

        return new OkObjectResult(randomString);
    }
}