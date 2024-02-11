using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Startup.Api.Helpers.Caching.Controllers.Interfaces;
using Startup.Api.Models.ApplicationSettings;

namespace Startup.Api.Helpers.Caching.Controllers;

/// <summary>
/// Protect the Canary page from aggressive hits.
/// Downtime on service may be as much as _cacheDurationSeconds before detection.
/// </summary>
public class AuthControllerCache : IAuthControllerCache
{
    private const string _cacheKey = "authControllerCache_";
    private readonly IMemoryCache _cache;
    private readonly int _cacheDurationSeconds;

    public AuthControllerCache(
        IMemoryCache cache,
        IOptions<AppSettings> appSettings)
    {
        _cache = cache;
        _cacheDurationSeconds = appSettings.Value.CacheDurationInSeconds;
    }

    public bool SetAuth(string key, string data)
    {
        try
        {
            _cache.Set($"{_cacheKey}{key}", data, TimeSpan.FromSeconds(_cacheDurationSeconds));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetAuth(string key)
    {
        if (_cache.TryGetValue($"{_cacheKey}{key}", out string data))
        {
            return data;
        }

        return null;
    }
}