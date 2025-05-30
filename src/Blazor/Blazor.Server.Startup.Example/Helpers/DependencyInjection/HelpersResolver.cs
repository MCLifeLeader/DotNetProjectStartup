using Startup.Blazor.Server.Models.ApplicationSettings;
using Microsoft.Extensions.Caching.Memory;

namespace Startup.Blazor.Server.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();
    }
}