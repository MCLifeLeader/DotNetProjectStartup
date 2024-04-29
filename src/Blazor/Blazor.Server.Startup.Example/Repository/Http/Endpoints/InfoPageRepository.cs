using System.Text;
using Startup.Blazor.Server.Repository.Http.Endpoints.Interfaces;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Blazor.Server.Repository.Http.Endpoints;

public class InfoPageRepository : IInfoPageRepository
{
    private readonly IInfoPageCache _cache;
    private readonly IHttpClientWrapper _httpClient;

    public InfoPageRepository(
        IHttpClientWrapper httpClientWrapper,
        IInfoPageCache cache)
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