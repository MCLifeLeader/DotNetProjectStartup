using React.Startup.Example.Models.Authorization;

namespace React.Startup.Example.Services.Interfaces;

public interface IAuthService
{
    string AuthenticateUser(UserLoginModel? user);
}