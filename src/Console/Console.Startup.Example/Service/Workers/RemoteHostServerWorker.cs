using System.Text;
using Console.Startup.Example.Model;
using Console.Startup.Example.Repositories.Http.Interface;
using Console.Startup.Example.Service.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Console.Startup.Example.Service.Workers;

public class RemoteHostServerWorker : IRemoteHostServerWorker
{
    private readonly AppSettings _appSettings;
    private readonly IRemoteHostServerRepository _remoteHostServerRepository;
    private readonly ILogger<RemoteHostServerWorker> _logger;

    public RemoteHostServerWorker(
        ILogger<RemoteHostServerWorker> logger,
        IOptions<AppSettings> appSettings,
        IRemoteHostServerRepository remoteHostServerRepository
        )
    {
        _logger = logger;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _remoteHostServerRepository = remoteHostServerRepository ?? throw new ArgumentNullException(nameof(remoteHostServerRepository));
    }

    public async Task ProcessRecordsNeedingUpdate(CancellationToken cancellationToken)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ProcessRecordsNeedingUpdate));
        _logger.LogInformation("Starting Worker: '{Class}.{Method}'", GetType().Name, nameof(ProcessRecordsNeedingUpdate));

        StringBuilder sb = new();

        DateTime endTime = DateTime.UtcNow + TimeSpan.FromMinutes(_appSettings.WorkerRunTimeMinutes);

        // Run for a while and then allow things to fall out of scope helping to support GC. The calling service will restart it.
        while (endTime > DateTime.UtcNow)
        {
            await Task.Yield();

            string mainHosts = string.Empty;
            string altHosts = string.Empty;

            try
            {
                try
                {
                    mainHosts = await _remoteHostServerRepository.GetMainHostsFileFromServer(cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log the error but let the application loop continue.
                    _logger.LogWarning(ex, ex.Message);
                }

                try
                {
                    altHosts = await _remoteHostServerRepository.GetAltHostsFileFromServer(Environment.MachineName.ToLower(), cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log the error but let the application loop continue.
                    _logger.LogWarning(ex, ex.Message);
                }

                if (!string.IsNullOrEmpty(mainHosts))
                {
                    sb.AppendLine("# Main Begin");
                    sb.AppendLine(mainHosts);
                    sb.AppendLine("# Main End");
                    sb.AppendLine("# Alt Begin");
                    sb.AppendLine(altHosts);
                    sb.AppendLine("# Alt End");

                    _logger.LogDebug(sb.ToString());

                    File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
                    var fileStream = File.Open(@"C:\Windows\System32\drivers\etc\hosts", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamWriter sw = new StreamWriter(fileStream);
                    await sw.WriteAsync(sb, cancellationToken);

                    await sw.FlushAsync();
                    await fileStream.FlushAsync(cancellationToken);
                    sw.Close();
                    fileStream.Close();
                }

                sb.Clear();
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

            // Play friendly with the API endpoint when there is no work to be done
            await Task.Delay(TimeSpan.FromSeconds(_appSettings.DataConnection.ApiSleepTimerSeconds), cancellationToken);
        }

        _logger.LogInformation("*** '{Class}.{Method}' Worker time limit reached, resetting worker ***",
            GetType().Name, nameof(ProcessRecordsNeedingUpdate));
    }
}