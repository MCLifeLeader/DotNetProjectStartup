using Api.Startup.Example.Models.ApplicationSettings;
using Api.Startup.Example.Services.Interfaces;

namespace Api.Startup.Example.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddTransient<IInfoService, InfoService>();
        service.AddTransient<IAuthService, AuthService>();
    }
}