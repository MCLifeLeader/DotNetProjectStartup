using Microsoft.Extensions.Caching.Memory;
using Startup.Api.Helpers.Caching.Controllers;
using Startup.Api.Helpers.Caching.Controllers.Interfaces;
using Startup.Api.Models.ApplicationSettings;

namespace Startup.Api.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();

        service.AddTransient<IAuthControllerCache, AuthControllerCache>();
    }
}