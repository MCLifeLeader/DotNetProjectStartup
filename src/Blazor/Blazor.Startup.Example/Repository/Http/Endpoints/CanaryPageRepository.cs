using System.Text;
using Blazor.Startup.Example.Connection.Interfaces;
using Blazor.Startup.Example.Constants;
using Blazor.Startup.Example.Repository.Http.Endpoints.Interfaces;

namespace Blazor.Startup.Example.Repository.Http.Endpoints;

public class CanaryPageRepository : ICanaryPageRepository
{
    private readonly ICanaryPageCache _cache;
    private readonly IHttpClientWrapper _httpClient;

    public CanaryPageRepository(
        IHttpClientWrapper httpClientWrapper,
        ICanaryPageCache cache)
    {
        _httpClient = httpClientWrapper;
        _cache = cache;
    }

    public string GetCanaryPage()
    {
        string canary = "canary";
        string cached = _cache.GetCanaryPage(canary);

        if (string.IsNullOrEmpty(cached))
        {
            string data = Encoding.UTF8.GetString(_httpClient.GetBytes("api/Info/StatusJson", HttpClientNames.STARTUPEXAMPLE_APP));

            _cache.SetCanaryPage(canary, data);
            return data;
        }

        return cached;
    }

    public string GetWeatherPage()
    {
        return Encoding.UTF8.GetString(_httpClient.GetBytes("api/v1.0/WeatherForecast/GetWeatherForecast2", HttpClientNames.STARTUPEXAMPLE_API));
    }

    public async Task<string> GetCanaryPageAsync()
    {
        string canary = "canary";
        string cached = _cache.GetCanaryPage(canary);

        if (string.IsNullOrEmpty(cached))
        {
            string data = Encoding.UTF8.GetString(await _httpClient.GetBytesAsync("api/Info/StatusJson", HttpClientNames.STARTUPEXAMPLE_API));

            _cache.SetCanaryPage(canary, data);
            return data;
        }

        return cached;
    }

    public async Task<string> GetWeatherPageAsync()
    {
        return Encoding.UTF8.GetString(await _httpClient.GetBytesAsync("api/v1.0/WeatherForecast/GetWeatherForecast2", HttpClientNames.STARTUPEXAMPLE_API));
    }
}