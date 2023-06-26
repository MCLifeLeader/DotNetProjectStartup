using Api.Startup.Example.Connection.Interfaces;
using Api.Startup.Example.Model.ApplicationSettings;

namespace Api.Startup.Example.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}