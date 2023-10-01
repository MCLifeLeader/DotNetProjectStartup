using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Startup.Client.Repositories.Http.StartupApi.Interfaces;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Startup.Client.Api;

public partial class StartupHttp
{
    private readonly UserLoginModel _userLogin;
    private readonly ILogger<StartupHttp> _logger;
    private readonly IApiHealthRepository _apiHealthRepository;

    /// <summary>
    /// Preferred Injected constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="apiHealthRepository"></param>
    /// <param name="httpClientWrapper"></param>
    /// <param name="userLogin"></param>
    public StartupHttp(
        ILogger<StartupHttp> logger,
        IApiHealthRepository apiHealthRepository,
        IHttpClientWrapper httpClientWrapper,
        UserLoginModel userLogin)
    {
        _logger = logger;

        _httpClient = httpClientWrapper.HttpClient(HttpClientNames.STARTUP_API);
        _baseUrl = _httpClient.BaseAddress?.OriginalString;
        _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        _apiHealthRepository = apiHealthRepository;
        _userLogin = userLogin;
    }

    public StartupHttp(string url, UserLoginModel login)
    {
        _baseUrl = url;
        _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        _httpClient = new HttpClient();

        _userLogin = login;

        _httpClient.BaseAddress = new Uri(url);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // ReSharper disable once VirtualMemberCallInConstructor
        AuthToken token = this.LoginAsync(_userLogin).Result;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
    }

    public void RefreshToken()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(RefreshToken));
        _logger.LogInformation("Refreshing token for '{AccountName}'", _userLogin.Username);

        AuthToken token = new();

        if (!string.IsNullOrEmpty(_userLogin.Username))
        {
            token = LoginAsync(_userLogin).Result;
        }
        else
        {
            token.Token = string.Empty;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
    }

    public async Task<string> CheckApiHealthAsync()
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(StartupHttp));

        var result = await _apiHealthRepository.GetHealthAsync();
        _logger.LogInformation("{healthStatus}", result);

        return result;
    }
}