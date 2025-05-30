using Startup.Blazor.Server.Repository.Http.Endpoints.Interfaces;
using Startup.Blazor.Server.Services.Interfaces;

namespace Startup.Blazor.Server.Services;

public class InfoService : IInfoService
{
    private readonly IInfoPageRepository _infoPageRepository;
    private readonly ILogger<InfoService> _logger;

    public InfoService(ILogger<InfoService> logger,
        IInfoPageRepository canaryPageRepository)
    {
        _logger = logger ?? throw new NullReferenceException($"{nameof(logger)} was null");
        _infoPageRepository = canaryPageRepository ?? throw new NullReferenceException($"{nameof(canaryPageRepository)} was null");
    }

    public string ReadApiCanaryPage()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ReadApiCanaryPage));

        string canary = _infoPageRepository.GetCanaryPage();
        string weather = string.Empty;//_canaryPageRepository.GetWeatherPage();

        return canary + weather;
    }

    public async Task<string> ReadApiCanaryPageAsync()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ReadApiCanaryPage));

        string canary = await _infoPageRepository.GetCanaryPageAsync();
        string weather = string.Empty;//await _canaryPageRepository.GetWeatherPageAsync();

        return canary + weather;
    }
}