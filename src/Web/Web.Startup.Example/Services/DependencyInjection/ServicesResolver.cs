using Startup.Web.Data;
using Startup.Web.Models.ApplicationSettings;
using Startup.Web.Services.Interfaces;

namespace Startup.Web.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddSingleton<WeatherForecastService>();
        service.AddTransient<IInfoService, InfoService>();
    }
}