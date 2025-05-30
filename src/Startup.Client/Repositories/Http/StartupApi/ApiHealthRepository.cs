using Microsoft.Extensions.Logging;
using Startup.Client.Repositories.Http.StartupApi.Interfaces;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;
using System.Text;

namespace Startup.Client.Repositories.Http.StartupApi;

public class ApiHealthRepository : IApiHealthRepository
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly ILogger<ApiHealthRepository> _logger;

    public ApiHealthRepository(ILogger<ApiHealthRepository> logger,
        IHttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClient = httpClientWrapper;
    }

    /// <summary>
    /// The health API endpoint is not published in the OpenAPI specification.
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetHealthAsync()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetHealthAsync));

        return Encoding.UTF8.GetString(await _httpClient.GetBytesAsync("_health", HttpClientNames.STARTUPEXAMPLE_API));
    }
}