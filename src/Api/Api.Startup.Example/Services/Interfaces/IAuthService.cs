using Api.Startup.Example.Models.Authorization;

namespace Api.Startup.Example.Services.Interfaces;

public interface IAuthService
{
    string AuthenticateUser(UserLoginModel? user);
}