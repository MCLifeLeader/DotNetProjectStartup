using Microsoft.Extensions.Caching.Memory;
using Startup.Web.Models.ApplicationSettings;

namespace Startup.Web.Helpers.DependencyInjection;

public static class HelpersResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<MemoryCache>();
    }
}