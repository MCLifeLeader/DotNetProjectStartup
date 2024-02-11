using Startup.Api.Models.ApplicationSettings;
using Startup.Api.Services.Interfaces;

namespace Startup.Api.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddTransient<IInfoService, InfoService>();
        service.AddTransient<IAuthService, AuthService>();
    }
}