using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Startup.Console.BackgroundService.Interface;
using Startup.Console.BackgroundService.Workers;
using Startup.Console.Constants;
using Startup.Console.Helpers.Extensions;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console.BackgroundService;

/// <summary>
/// This is the main service that will run all the background workers.
/// </summary>
public class HostedBackgroundServices : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<HostedBackgroundServices> _logger;
    private readonly IHealthCheckWorker _healthCheckWorker;
    private readonly IFeatureManager _featureManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HostedBackgroundServices(
        IFeatureManager featureManager,
        ILogger<HostedBackgroundServices> logger,
        IOptions<AppSettings> appSettings,
        IHealthCheckWorker healthCheckWorker)
    {
        _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _healthCheckWorker = healthCheckWorker ?? throw new ArgumentNullException(nameof(healthCheckWorker));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ExecuteAsync));
        _logger.LogWarning("{Service} is starting. - {dateTime}", nameof(HostedBackgroundServices), DateTime.UtcNow);
        _logger.LogAppSettings(_appSettings);

        // Add collection of feature flags here that enable / disable worker threads
        bool healthCheckWorkerEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.HEALTH_CHECK_WORKER);

        List<Task> tasks = new List<Task>();

        try
        {
            _logger.LogInformation("Starting the Workers");

            // Run the service until a cancellation token has been raised.
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();

                #region Worker threads can be added here

                if (healthCheckWorkerEnabled)
                {
                    _logger.LogWarning($"Worker {nameof(HealthCheckWorker.CheckStartupApi)} has been Started");
                    tasks.Add(_healthCheckWorker.CheckStartupApi(cancellationToken));
                }
                else
                {
                    _logger.LogWarning($"Worker {nameof(HealthCheckWorker.CheckStartupApi)} has been Disabled");
                }

                Task.WaitAll(tasks.ToArray());
                tasks.Clear();

                #endregion

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(_appSettings.WorkerProcesses.SleepDelaySeconds), cancellationToken);
            }
        }
        catch (TaskCanceledException ex)
        {
            // This will most likely happen on a "Ctrl-C" or other shutdown request to the service.
            _logger.LogWarning(ex, "Abrupt shutdown encountered.");
        }
        catch (Exception ex)
        {
            // This is type of error is unexpected and should be investigated.
            _logger.LogError(ex, "Unexpected error encountered.");
            Environment.Exit(1);
        }

        _logger.LogWarning("{Service} is stopping. - {dateTime}", nameof(HostedBackgroundServices), DateTime.UtcNow);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(StartAsync));
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(HostedBackgroundServices), DateTime.UtcNow);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(StopAsync));
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(HostedBackgroundServices), DateTime.UtcNow);
        await base.StopAsync(cancellationToken);
    }
}