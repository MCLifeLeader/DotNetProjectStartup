using Microsoft.Extensions.DependencyInjection;
using Startup.Client.Repositories.Http.StartupApi;
using Startup.Client.Repositories.Http.StartupApi.Interfaces;
using Startup.Console.Repositories.Http;
using Startup.Console.Repositories.Http.Interface;

namespace Startup.Console.Repositories.DependencyInjection;

public static class RepositoriesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddTransient<IRemoteHostServerRepository, RemoteHostServerRepository>();
        services.AddTransient<IApiHealthRepository, ApiHealthRepository>();
    }
}