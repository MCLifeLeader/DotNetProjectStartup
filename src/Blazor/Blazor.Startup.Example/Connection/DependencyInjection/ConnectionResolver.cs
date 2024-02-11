using Startup.Blazor.Server.Models.ApplicationSettings;
using Startup.Common.Connection;
using Startup.Common.Connection.Interfaces;

namespace Startup.Blazor.Server.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}