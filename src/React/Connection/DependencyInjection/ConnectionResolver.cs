using React.Startup.Example.Connection.Interfaces;
using React.Startup.Example.Models.ApplicationSettings;

namespace React.Startup.Example.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}