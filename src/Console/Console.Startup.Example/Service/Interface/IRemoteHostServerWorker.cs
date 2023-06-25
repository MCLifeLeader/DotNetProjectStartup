namespace Console.Startup.Example.Service.Interface;

public interface IRemoteHostServerWorker
{
    Task ProcessRecordsNeedingUpdate(CancellationToken cancellationToken);
}