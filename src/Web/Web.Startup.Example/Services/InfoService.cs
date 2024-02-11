using Startup.Web.Repository.Http.Endpoints.Interfaces;
using Startup.Web.Services.Interfaces;

namespace Startup.Web.Services;

public class InfoService : IInfoService
{
    private readonly IInfoPageRepository _canaryPageRepository;
    private readonly ILogger<InfoService> _logger;

    public InfoService(ILogger<InfoService> logger,
        IInfoPageRepository canaryPageRepository)
    {
        _logger = logger ?? throw new NullReferenceException($"{nameof(logger)} was null");
        _canaryPageRepository = canaryPageRepository ?? throw new NullReferenceException($"{nameof(canaryPageRepository)} was null");
    }

    public string ReadApiCanaryPage()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ReadApiCanaryPage));

        string canary = _canaryPageRepository.GetCanaryPage();
        string weather = _canaryPageRepository.GetWeatherPage();

        return canary + weather;
    }

    public async Task<string> ReadApiCanaryPageAsync()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ReadApiCanaryPage));

        string canary = await _canaryPageRepository.GetCanaryPageAsync();
        string weather = await _canaryPageRepository.GetWeatherPageAsync();

        return canary + weather;
    }
}