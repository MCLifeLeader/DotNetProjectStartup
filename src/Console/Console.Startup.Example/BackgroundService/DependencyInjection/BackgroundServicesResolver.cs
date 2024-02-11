using Microsoft.Extensions.DependencyInjection;
using Startup.Console.BackgroundService.Interface;
using Startup.Console.BackgroundService.Workers;

namespace Startup.Console.BackgroundService.DependencyInjection;

public static class BackgroundServicesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IHealthCheckWorker, HealthCheckWorker>();
        services.AddSingleton<IFileDownloadWorker, FileDownloadWorker>();
        services.AddHostedService<HostedBackgroundServices>();
    }
}