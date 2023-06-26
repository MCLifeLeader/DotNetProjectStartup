using Api.Startup.Example.Model.ApplicationSettings;

namespace Api.Startup.Example.Factories.DependencyInjection;

public static class FactoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        //service.AddScoped<IFactory, Factory>();
    }
}