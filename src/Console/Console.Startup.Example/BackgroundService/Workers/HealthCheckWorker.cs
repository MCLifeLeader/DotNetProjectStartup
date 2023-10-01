using Console.Startup.Example.BackgroundService.Interface;
using Console.Startup.Example.Model.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Startup.Client.Api;

namespace Console.Startup.Example.BackgroundService.Workers;

public class HealthCheckWorker : IHealthCheckWorker
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<HealthCheckWorker> _logger;
    private readonly StartupHttp _startupHttp;

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
                _startupHttp.RefreshToken();
                var apiHealth = await _startupHttp.CheckApiHealthAsync();
                _logger.LogInformation($"{apiHealth}");
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

            NCrontab.CrontabSchedule schedule = NCrontab.CrontabSchedule.Parse(_appSettings.WorkerProcesses.StartupApi.Cron);
            DateTime nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);

            _logger.LogInformation("Thread Sleep for: '{Class}.{Method}' until: {nextRun}",
                GetType().Name, nameof(CheckStartupApi), nextRun);

            while (DateTime.UtcNow < nextRun)
            {
                // Play friendly with the API endpoint when there is no work to be done
                await Task.Delay(TimeSpan.FromSeconds(_appSettings.WorkerProcesses.SleepDelaySeconds), cancellationToken);
            }
            _logger.LogInformation("Thread Resumed for: '{Class}.{Method}'", GetType().Name, nameof(CheckStartupApi));
        }
    }
}