using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Api.Startup.Example.Models.ApplicationSettings;
using Api.Startup.Example.Models.Authorization;
using Api.Startup.Example.Models.Enums;
using Api.Startup.Example.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Startup.Data.Models.Db.dboSchema;
using Startup.Data.Repositories.Db.Interfaces;

namespace Api.Startup.Example.Services;

public class AuthService : IAuthService
{
    private readonly AppSettings _appSettings;
    private readonly IAuthenticationLogRepository _authenticationLogRepository;
    private readonly ILogger<AuthService> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IStartupExampleContext _startupExampleContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(
        ILogger<AuthService> logger,
        IOptions<AppSettings> appSettings,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IStartupExampleContext startupExampleContext,
        IAuthenticationLogRepository authenticationLogRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _startupExampleContext = startupExampleContext ?? throw new ArgumentNullException(nameof(startupExampleContext));
        _authenticationLogRepository = authenticationLogRepository ?? throw new ArgumentNullException(nameof(authenticationLogRepository));
        ;
    }

    //ToDo: Update the method to return AuthToken instead of a string
    public string AuthenticateUser(UserLoginModel? user)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(AuthenticateUser));

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return GenerateJsonWebToken(ValidateUserCredentials(user));
    }

    private IList<Claim> ValidateUserCredentials(UserLoginModel? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        AuthenticationLog authLog = null;

        List<Claim> claims = new List<Claim>();

        IdentityUser iUser = _userManager.FindByNameAsync(user.Username).Result;

        _userManager.ChangePasswordAsync(iUser, "P@ssword123", "P@ssword123").GetAwaiter().GetResult();

        // Create a log entry for the authentication attempt.
        authLog = new AuthenticationLog
        {
            Username = user.Username,
            AuthStatusId = (short) AuthenticationStatusEnum.Failure,
        };

        if (iUser != null && _userManager.CheckPasswordAsync(iUser, user.Password).Result)
        {
            // Update the log entry with the user Id.
            authLog.AspNetUsersId = iUser.Id;

            IList<string> roles = _userManager.GetRolesAsync(iUser).Result;
            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            else
            {
                _logger.LogError("Invalid Login Attempt by {userLogin}", user.Username);

                _authenticationLogRepository.Add(authLog);
                _startupExampleContext.SaveChanges();

                throw new SecurityException("Unauthorized Login Attempt");
            }

            DateTime tokenDateTime = DateTime.UtcNow;
            DateTime expTokenDateTime = tokenDateTime.Add(TimeSpan.FromMinutes(_appSettings.Jwt.ExpireInMinutes));

            claims.Add(new Claim(ClaimTypes.Sid, iUser.Id));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
            claims.Add(new Claim(ClaimTypes.Name, user.DisplayName ?? string.Empty));
            claims.Add(new Claim(ClaimTypes.Expiration, expTokenDateTime.ToString("O")));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Jwt.Subject));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, iUser.Email ?? string.Empty));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(tokenDateTime.ToUniversalTime()).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(tokenDateTime.ToUniversalTime()).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expTokenDateTime.ToUniversalTime()).ToString()));

            // Update the log entry with the success status.
            authLog.AuthStatusId = (short) AuthenticationStatusEnum.Success;

            _authenticationLogRepository.Add(authLog);
            _startupExampleContext.SaveChanges();
        }
        else
        {
            _logger.LogError("Invalid Login Attempt by {userLogin}", user.Username);

            _authenticationLogRepository.Add(authLog);
            _startupExampleContext.SaveChanges();

            throw new SecurityException("Unauthorized Login Attempt");
        }

        return claims;
    }

    private string GenerateJsonWebToken(IList<Claim> claims)
    {
        ClaimsIdentity identity = new ClaimsIdentity(claims, "Token");
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new JwtSecurityToken(
            _appSettings.Jwt.Issuer,
            _appSettings.Jwt.Audience,
            identity.Claims,
            DateTime.Now.AddMinutes(_appSettings.Jwt.ExpireInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}