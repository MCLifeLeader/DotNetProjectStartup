using Startup.Web.Models.ApplicationSettings;

namespace Startup.Web.Factories.DependencyInjection;

public static class FactoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        //service.AddScoped<IFactory, Factory>();
    }
}