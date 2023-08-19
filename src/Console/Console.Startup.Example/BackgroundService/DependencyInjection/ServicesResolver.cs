using Console.Startup.Example.Service.Interface;
using Console.Startup.Example.Service.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Console.Startup.Example.Service.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IRemoteHostServerWorker, RemoteHostServerWorker>();
        services.AddHostedService<RemoteHostServerService>();
    }
}