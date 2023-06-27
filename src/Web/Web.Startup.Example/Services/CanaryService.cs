using Web.Startup.Example.Repository.Http.Endpoints.Interfaces;
using Web.Startup.Example.Services.Interfaces;

namespace Web.Startup.Example.Services;

public class CanaryService : ICanaryService
{
    private readonly ICanaryPageRepository _canaryPageRepository;
    private readonly ILogger<CanaryService> _logger;

    public CanaryService(ILogger<CanaryService> logger,
        ICanaryPageRepository canaryPageRepository)
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