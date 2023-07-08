using React.Startup.Example.Models.ApplicationSettings;
using React.Startup.Example.Services.Interfaces;

namespace React.Startup.Example.Services.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddTransient<IInfoService, InfoService>();
    }
}