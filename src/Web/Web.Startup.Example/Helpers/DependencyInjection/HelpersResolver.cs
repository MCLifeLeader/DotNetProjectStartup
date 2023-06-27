using Microsoft.Extensions.Caching.Memory;
using Web.Startup.Example.Models.ApplicationSettings;

namespace Web.Startup.Example.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();
    }
}