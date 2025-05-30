using Startup.Api.Models.ApplicationSettings;

namespace Startup.Api.Factories.DependencyInjection;

public static class FactoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        // ToDo: Register your factories here

        //service.AddScoped<IFactory, Factory>();
    }
}