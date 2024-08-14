using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Startup.Client.Api;
using Startup.Console.BackgroundService.Interface;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console.BackgroundService.Workers;

public class HealthCheckWorker : IHealthCheckWorker
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<HealthCheckWorker> _logger;
    private readonly StartupHttp _startupHttp;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HealthCheckWorker(
        ILogger<HealthCheckWorker> logger,
        IOptions<AppSettings> appSettings,
        StartupHttp startupHttp)
    {
        _logger = logger;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _startupHttp = startupHttp ?? throw new ArgumentNullException(nameof(startupHttp));
    }

    public async Task CheckStartupApi(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(CheckStartupApi));
        _logger.LogInformation("Starting Worker: '{Class}.{Method}'", GetType().Name, nameof(CheckStartupApi));

        // Run for a while and then allow things to fall out of scope helping to support GC. The calling service will restart it.
        while (true)
        {
            try
            {
                _logger.LogInformation("{appSettings}", _appSettings);

                // Sleep until cron rule has been satisfied.
                NCrontab.CrontabSchedule schedule = NCrontab.CrontabSchedule.Parse(_appSettings.WorkerProcesses.StartupApi.Cron);
                DateTime nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);

                _logger.LogInformation("Thread Sleep for: '{Class}.{Method}' until: {nextRun}",
                    GetType().Name, nameof(CheckStartupApi), nextRun);

                while (DateTime.UtcNow < nextRun)
                {
                    // Play friendly with the API endpoint when there is no work to be done
                    await Task.Delay(TimeSpan.FromSeconds(_appSettings.WorkerProcesses.SleepDelaySeconds), cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogWarning("'{Class}.{Method}' CancellationToken Received", GetType().Name, nameof(CheckStartupApi));
                        return;
                    }
                }

                _logger.LogInformation("Thread Resumed for: '{Class}.{Method}'", GetType().Name, nameof(CheckStartupApi));
            }
            catch (Exception ex)
            {
                // Log the error but let the application loop continue.
                _logger.LogCritical(ex, $"Cron failed for {nameof(CheckStartupApi)}");
            }

            try
            {
                // Run the desired action.
                _startupHttp.RefreshToken();
                var apiHealth = await _startupHttp.CheckApiHealthAsync();
                _logger.LogInformation($"{apiHealth}");
            }
            catch (TaskCanceledException)
            {
                // Some stop action happened, throw the error up stream.
                throw;
            }
            catch (Exception ex)
            {
                // Log the error but let the application loop continue.
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}