using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Startup.Console.BackgroundService.Interface;
using Startup.Console.Model.ApplicationSettings;
using Startup.Console.Repositories.Http.Interface;

namespace Startup.Console.BackgroundService.Workers;

public class FileDownloadWorker : IFileDownloadWorker
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<FileDownloadWorker> _logger;
    private readonly IRemoteHostServerRepository _remoteHostServerRepository;

    public FileDownloadWorker(
        ILogger<FileDownloadWorker> logger,
        IOptions<AppSettings> appSettings,
        IRemoteHostServerRepository remoteHostServerRepository)
    {
        _logger = logger;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _remoteHostServerRepository = remoteHostServerRepository ?? throw new ArgumentNullException(nameof(remoteHostServerRepository));
    }

    public async Task ProcessDownloadFile(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ProcessDownloadFile));
        _logger.LogInformation("Starting Worker: '{Class}.{Method}'", GetType().Name, nameof(ProcessDownloadFile));

        // Run for a while and then allow things to fall out of scope helping to support GC. The calling service will restart it.
        while (true)
        {
            try
            {
                string urlPath = "MCLifeLeader/DotNetProjectStartup/blob/main/README.md";
                var details = await _remoteHostServerRepository.GetFileFromServer(urlPath, cancellationToken);
                _logger.LogInformation($"Reading File - {urlPath} : Contents - {details}");
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the error but let the application loop continue.
                _logger.LogError(ex, ex.Message);
            }

            NCrontab.CrontabSchedule schedule = NCrontab.CrontabSchedule.Parse(_appSettings.WorkerProcesses.RemoteServerConnection.Cron);
            DateTime nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);

            _logger.LogInformation("Thread Sleep for: '{Class}.{Method}' until: {nextRun}",
                GetType().Name, nameof(ProcessDownloadFile), nextRun);

            while (DateTime.UtcNow < nextRun)
            {
                // Play friendly with the API endpoint when there is no work to be done
                await Task.Delay(TimeSpan.FromSeconds(_appSettings.WorkerProcesses.SleepDelaySeconds), cancellationToken);
            }
            _logger.LogInformation("Thread Resumed for: '{Class}.{Method}'", GetType().Name, nameof(ProcessDownloadFile));
        }
    }
}
