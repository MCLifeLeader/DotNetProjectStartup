using Api.Startup.Example.Models.ApplicationSettings;

namespace Api.Startup.Example.Factories.DependencyInjection;

public static class FactoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        // ToDo: Register your factories here

        //service.AddScoped<IFactory, Factory>();
    }
}