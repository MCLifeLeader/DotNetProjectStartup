using Console.Startup.Example.Repositories.Http;
using Console.Startup.Example.Repositories.Http.Interface;
using Microsoft.Extensions.DependencyInjection;
using Startup.Client.Repositories.Http.StartupApi.Interfaces;
using Startup.Client.Repositories.Http.StartupApi;

namespace Console.Startup.Example.Repositories.DependencyInjection;

public static class RepositoriesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddTransient<IRemoteHostServerRepository, RemoteHostServerRepository>();
        services.AddTransient<IApiHealthRepository, ApiHealthRepository>();
    }
}