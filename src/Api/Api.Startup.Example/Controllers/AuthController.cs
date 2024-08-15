using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startup.Api.Helpers.Caching.Controllers.Interfaces;
using Startup.Api.Models;
using Startup.Api.Services.Interfaces;
using Startup.Common.Models.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security;

namespace Startup.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthControllerCache _authControllerCache;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        IAuthControllerCache authControllerCache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _authControllerCache = authControllerCache ?? throw new ArgumentNullException(nameof(authControllerCache));
    }

    /// <summary>
    /// User account login endpoint. 
    /// </summary>
    /// <param name="user">Contains details about the user logging in.</param>
    /// <returns>Token if the login is successful</returns>
    [HttpPost("Login")]
    [Produces("application/json")]
    [SwaggerResponse((int) HttpStatusCode.OK, "Returns the JWT if Successful", typeof(AuthToken))]
    public async Task<ActionResult<AuthToken>> Login([FromBody] UserLoginModel? user)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Login));
        await Task.Yield();

        long logId = DateTime.UtcNow.Ticks;

        if (user == null)
        {
            return BadRequest("No Body Post Message");
        }

        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Body missing Username or Password");
        }

        try
        {
            string? tokenString = _authControllerCache.GetAuth($"{user.Username}");

            if (string.IsNullOrEmpty(tokenString))
            {
                // Add the clientId to the model
                tokenString = _authService.AuthenticateUser(user);
            }

            // A null token string is a failure to login. Do not Cache this.
            if (!string.IsNullOrEmpty(tokenString))
            {
                _authControllerCache.SetAuth($"{user.Username}", tokenString);
                return Ok(new AuthToken {Token = tokenString});
            }

            ErrorResult errorMessage = new ErrorResult {LogId = logId.ToString(), Message = "Invalid Login Credentials"};
            _logger.LogWarning("LogId:{logId} - Message:{message}", errorMessage.LogId, errorMessage.Message);

            return Unauthorized(errorMessage);
        }
        catch (SecurityException ex)
        {
            ErrorResult errorMessage = new ErrorResult {LogId = logId.ToString(), Message = ex.Message};
            _logger.LogCritical(ex, "{logId} - Failed to Login - {message}", errorMessage.LogId, errorMessage.Message);

            return Unauthorized(errorMessage);
        }
        catch (Exception ex)
        {
            ErrorResult errorMessage = new ErrorResult {LogId = logId.ToString(), Message = ex.Message};
            _logger.LogCritical(ex, "{logId} - Failed to Login - {message}", errorMessage.LogId, errorMessage.Message);

            return Problem(ex.Message, null, (int) HttpStatusCode.InternalServerError, $"LogId:{logId}");
        }
    }
}