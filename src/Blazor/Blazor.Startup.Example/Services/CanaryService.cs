using Blazor.Startup.Example.Repository.Http.Endpoints.Interfaces;
using Blazor.Startup.Example.Services.Interfaces;

namespace Blazor.Startup.Example.Services;

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
        string weather = string.Empty;//_canaryPageRepository.GetWeatherPage();

        return canary + weather;
    }

    public async Task<string> ReadApiCanaryPageAsync()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(ReadApiCanaryPage));

        string canary = await _canaryPageRepository.GetCanaryPageAsync();
        string weather = string.Empty;//await _canaryPageRepository.GetWeatherPageAsync();

        return canary + weather;
    }
}