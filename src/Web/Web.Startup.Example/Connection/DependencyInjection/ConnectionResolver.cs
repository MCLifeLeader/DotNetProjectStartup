using Web.Startup.Example.Connection.Interfaces;
using Web.Startup.Example.Models.ApplicationSettings;

namespace Web.Startup.Example.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}