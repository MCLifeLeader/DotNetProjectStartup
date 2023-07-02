using Console.Startup.Example.Helpers.Extensions;
using Console.Startup.Example.Model;
using Console.Startup.Example.Service.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Console.Startup.Example.Service;

public class RemoteHostServerService : BackgroundService
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<RemoteHostServerService> _logger;
    private readonly IRemoteHostServerWorker _worker;

    public RemoteHostServerService(
        ILogger<RemoteHostServerService> logger,
        IOptions<AppSettings> appSettings,
        IRemoteHostServerWorker worker)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _worker = worker ?? throw new ArgumentNullException(nameof(worker));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));

        _logger.LogInformation("Connected To Service Url {url}", _appSettings.DataConnection.Uri);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Service} is starting. - {dateTime}", nameof(IRemoteHostServerWorker), DateTime.UtcNow);
        _logger.LogInformation(await _appSettings.ToJsonAsync());

        List<Task> tasks = new List<Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();

                _logger.LogInformation("Starting the Workers");

                //NCrontab.CrontabSchedule schedule = NCrontab.CrontabSchedule.Parse("*/5 * * * *");
                //var nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);
                //_logger.LogDebug($"{DateTime.UtcNow} < {nextRun}");
                
                #region Worker threads can be added here

                tasks.Add(_worker.ProcessRecordsNeedingUpdate(cancellationToken));
                //tasks.Add(_worker.Process2(cancellationToken));
                //tasks.Add(_worker.Process3(cancellationToken));

                Task.WaitAll(tasks.ToArray());
                tasks.Clear();

                #endregion

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(_appSettings.ServiceRunTimeSleepDelaySeconds), cancellationToken);
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
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(IRemoteHostServerWorker), DateTime.UtcNow);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Service} is stopping. - {dateTime}", nameof(IRemoteHostServerWorker), DateTime.UtcNow);
        await base.StopAsync(cancellationToken);
    }
}