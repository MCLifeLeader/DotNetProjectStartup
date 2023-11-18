using Blazor.Startup.Example.Models.ApplicationSettings;
using Blazor.Startup.Example.Services.Interfaces;

namespace Blazor.Startup.Example.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddTransient<ICanaryService, CanaryService>();
    }
}