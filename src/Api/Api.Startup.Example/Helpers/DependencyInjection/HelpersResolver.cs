using Api.Startup.Example.Helpers.Caching.Controllers;
using Api.Startup.Example.Helpers.Caching.Controllers.Interfaces;
using Api.Startup.Example.Models.ApplicationSettings;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Startup.Example.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();

        service.AddTransient<IAuthControllerCache, AuthControllerCache>();
    }
}