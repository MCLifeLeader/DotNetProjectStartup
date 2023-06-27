using Web.Startup.Example.Data;
using Web.Startup.Example.Models.ApplicationSettings;
using Web.Startup.Example.Services.Interfaces;

namespace Web.Startup.Example.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<WeatherForecastService>();
        service.AddTransient<ICanaryService, CanaryService>();
    }
}