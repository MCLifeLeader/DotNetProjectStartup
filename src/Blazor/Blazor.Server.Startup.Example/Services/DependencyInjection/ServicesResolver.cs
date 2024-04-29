using Startup.Blazor.Server.Models.ApplicationSettings;
using Startup.Blazor.Server.Services.Interfaces;

namespace Startup.Blazor.Server.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddTransient<IInfoService, InfoService>();
    }
}