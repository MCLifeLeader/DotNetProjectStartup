using Api.Startup.Example.Model.Authorization;

namespace Api.Startup.Example.Services.Interfaces;

public interface IAuthService
{
    string AuthenticateUser(UserLoginModel user);
}