using Blazor.Startup.Example.Models.ApplicationSettings;
using Microsoft.Extensions.Caching.Memory;

namespace Blazor.Startup.Example.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        //service.AddSingleton<MemoryCache>();
    }
}