using Console.Startup.Example.Connection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Console.Startup.Example.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection service)
    {
        service.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
    }
}