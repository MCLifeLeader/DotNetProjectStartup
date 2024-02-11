namespace Startup.Console.BackgroundService.Interface;

public interface IFileDownloadWorker
{
    Task ProcessDownloadFile(CancellationToken cancellationToken);
}