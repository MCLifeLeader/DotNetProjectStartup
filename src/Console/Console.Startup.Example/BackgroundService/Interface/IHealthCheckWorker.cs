namespace Console.Startup.Example.BackgroundService.Interface;

public interface IHealthCheckWorker
{
    Task CheckAdverTranApi(CancellationToken cancellationToken);
}