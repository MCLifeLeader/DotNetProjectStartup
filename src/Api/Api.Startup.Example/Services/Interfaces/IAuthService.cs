using Startup.Common.Models.Authorization;

namespace Startup.Api.Services.Interfaces;

public interface IAuthService
{
    string? AuthenticateUser(UserLoginModel? user);
}