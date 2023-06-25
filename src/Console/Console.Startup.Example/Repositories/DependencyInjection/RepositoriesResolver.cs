using Console.Startup.Example.Repositories.Http;
using Console.Startup.Example.Repositories.Http.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Console.Startup.Example.Repositories.DependencyInjection;

public static class RepositoriesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddTransient<IRemoteHostServerRepository, RemoteHostServerRepository>();
    }
}