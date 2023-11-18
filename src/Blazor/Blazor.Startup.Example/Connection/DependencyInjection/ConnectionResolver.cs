using Blazor.Startup.Example.Connection.Interfaces;
using Blazor.Startup.Example.Models.ApplicationSettings;

namespace Blazor.Startup.Example.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}