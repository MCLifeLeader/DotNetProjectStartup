using Console.Startup.Example.BackgroundService.Interface;
using Console.Startup.Example.Constants;
using Console.Startup.Example.Helpers.Extensions;
using Console.Startup.Example.Model.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace Console.Startup.Example.BackgroundService;

/// <summary>
/// This is the main service that will run all of the background workers.
/// </summary>
public class HostedBackgroundServices : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<HostedBackgroundServices> _logger;
    private readonly IHealthCheckWorker _healthCheckWorker;
    private readonly IFeatureManager _featureManager;

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
        _logger.LogInformation("{Service} is starting. - {dateTime}", nameof(IHealthCheckWorker), DateTime.UtcNow);
        _logger.LogInformation(await _appSettings.ToJsonAsync());

        bool healthCheckWorkerEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.HEALTH_CHECK_WORKER);

        List<Task> tasks = new List<Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();

                _logger.LogInformation("Starting the Workers");

                #region Worker threads can be added here

                if (healthCheckWorkerEnabled)
                {
                    tasks.Add(_healthCheckWorker.CheckAdverTranApi(cancellationToken));
                }
                else
                {
                    _logger.LogInformation($"");
                }

                //tasks.Add(_worker.Process2(cancellationToken));
                //tasks.Add(_worker.Process3(cancellationToken));

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
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(IHealthCheckWorker), DateTime.UtcNow);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(IHealthCheckWorker), DateTime.UtcNow);
        await base.StopAsync(cancellationToken);
    }
}