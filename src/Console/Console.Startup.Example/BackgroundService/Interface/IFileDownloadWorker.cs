namespace Console.Startup.Example.BackgroundService.Interface;

public interface IFileDownloadWorker
{
    Task ProcessDownloadFile(CancellationToken cancellationToken);
}