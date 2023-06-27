using System.Text;
using Web.Startup.Example.Connection.Interfaces;
using Web.Startup.Example.Constants;
using Web.Startup.Example.Repository.Http.Endpoints.Interfaces;

namespace Web.Startup.Example.Repository.Http.Endpoints;

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
            string data = Encoding.UTF8.GetString(_httpClient.GetBytes("v1.0/Canary/StatusJson", HttpClientNames.StartupExample_App));

            _cache.SetCanaryPage(canary, data);
            return data;
        }

        return cached;
    }

    public string GetWeatherPage()
    {
        return Encoding.UTF8.GetString(_httpClient.GetBytes("v1.0/WeatherForecast/GetWeatherForecast2", HttpClientNames.StartupExample_Api));
    }

    public async Task<string> GetCanaryPageAsync()
    {
        string canary = "canary";
        string cached = _cache.GetCanaryPage(canary);

        if (string.IsNullOrEmpty(cached))
        {
            string data = Encoding.UTF8.GetString(await _httpClient.GetBytesAsync("v1.0/Canary/StatusJson", HttpClientNames.StartupExample_Api));

            _cache.SetCanaryPage(canary, data);
            return data;
        }

        return cached;
    }

    public async Task<string> GetWeatherPageAsync()
    {
        return Encoding.UTF8.GetString(await _httpClient.GetBytesAsync("v1.0/WeatherForecast/GetWeatherForecast2", HttpClientNames.StartupExample_Api));
    }
}