using Microsoft.Extensions.Caching.Memory;
using Web.Startup.Example.Repository.Http.Endpoints.Interfaces;

namespace Web.Startup.Example.Repository.Http.Endpoints;

/// <summary>
/// Protect the Canary page from aggressive hits.
/// Downtime on service may be as much as _cacheDurationSeconds before detection.
/// </summary>
public class CanaryPageCache : ICanaryPageCache
{
    private const string _cacheKey = "StartupExampleApi_";
    private readonly IMemoryCache _cache;
    private readonly int _cacheDurationSeconds;

    public CanaryPageCache(IMemoryCache cache)
    {
        _cache = cache;
        _cacheDurationSeconds = 10;
    }

    /// <summary>
    /// Set the canary page cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool SetCanaryPage(string key, string data)
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

    /// <summary>
    /// Get the canary page cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetCanaryPage(string key)
    {
        if (_cache.TryGetValue($"{_cacheKey}{key}", out string data))
        {
            return data;
        }

        return null;
    }
}