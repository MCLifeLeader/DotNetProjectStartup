namespace Startup.Console.BackgroundService.Interface;

public interface IHealthCheckWorker
{
    Task CheckStartupApi(CancellationToken cancellationToken);
}