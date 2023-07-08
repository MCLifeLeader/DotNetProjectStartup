using Microsoft.Extensions.Caching.Memory;
using React.Startup.Example.Helpers.Caching.Controllers;
using React.Startup.Example.Helpers.Caching.Controllers.Interfaces;
using React.Startup.Example.Models.ApplicationSettings;

namespace React.Startup.Example.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();

        service.AddTransient<IAuthControllerCache, AuthControllerCache>();
    }
}