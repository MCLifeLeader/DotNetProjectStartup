using Startup.Common.Connection;
using Startup.Common.Connection.Interfaces;
using Startup.Web.Models.ApplicationSettings;

namespace Startup.Web.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }
}