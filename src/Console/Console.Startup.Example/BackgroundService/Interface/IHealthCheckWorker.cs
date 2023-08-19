namespace AdverTran.Background.Client.BackgroundServices.Interface;

public interface IHealthCheckWorker
{
    Task CheckAdverTranApi(CancellationToken cancellationToken);
}