using Console.Startup.Example.BackgroundService.Interface;
using Console.Startup.Example.BackgroundService.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Console.Startup.Example.BackgroundService.DependencyInjection;

public static class ServicesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IHealthCheckWorker, HealthCheckWorker>();
        services.AddHostedService<HostedBackgroundServices>();
    }
}