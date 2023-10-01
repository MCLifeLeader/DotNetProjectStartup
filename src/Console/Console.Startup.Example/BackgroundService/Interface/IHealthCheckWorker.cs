namespace Console.Startup.Example.BackgroundService.Interface;

public interface IHealthCheckWorker
{
    Task CheckStartupApi(CancellationToken cancellationToken);
}