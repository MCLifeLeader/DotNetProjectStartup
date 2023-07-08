using System.Net;
using System.Net.Http.Headers;
using React.Startup.Example.Connection.Interfaces;

namespace React.Startup.Example.Connection;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpClientWrapper> _logger;

    public HttpClientWrapper(
        ILogger<HttpClientWrapper> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public byte[] GetBytes(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetBytes));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.GetAsync(resourcePath).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsByteArrayAsync().Result;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public async Task<byte[]> GetBytesAsync(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetBytesAsync));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.GetAsync(resourcePath);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public T GetObjectUsingAccessToken<T>(string resourcePath, string clientName)
    {
        throw new NotImplementedException();
    }

    public T GetObject<T>(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<T>' called", GetType().Name, nameof(GetObject));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.GetAsync(resourcePath).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsAsync<T>().Result;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public async Task<T> GetObjectAsync<T>(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<T>' called", GetType().Name, nameof(GetObjectAsync));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.GetAsync(resourcePath);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public string GetClientCredentialToken()
    {
        throw new NotImplementedException();
    }

    public T GetObjectUsingBearerToken<T>(string resourcePath, string clientName, string token)
    {
        _logger.LogDebug("'{Class}.{Method}<T>' called", GetType().Name, nameof(GetObjectUsingBearerToken));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpRequestMessage request = new();
        request.Headers.Add("Accept", "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        request.Method = HttpMethod.Get;
        string url = $"{httpClient.BaseAddress}{resourcePath}";
        request.RequestUri = new Uri(url);


        HttpResponseMessage response = httpClient.SendAsync(request).Result;
        if (response.IsSuccessStatusCode)
        {
            Task<string> result = response.Content.ReadAsStringAsync();
            return response.Content.ReadAsAsync<T>().Result;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        string msg = $"GET:{resourcePath} erred out with a result: {response.StatusCode}";
        throw new Exception(msg);
    }

    public string PostData(string resourcePath, object o, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(PostData));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.PostAsJsonAsync(resourcePath, o).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        string msg =
            $"Post:{resourcePath} errored out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public string PutData(string resourcePath, object o, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(PutData));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.PutAsJsonAsync(resourcePath, o).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        string msg;
        try
        {
            msg = response.Content.ReadAsStringAsync().Result;
        }
        catch (Exception ex)
        {
            msg = $"Unable to read the response message {ex}";
        }

        throw new Exception($"Put:{resourcePath} erred out with a code of {response.StatusCode} and a message of:\n {msg}");
    }

    public T Post<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(Post));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        return PostAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    public async Task<T> PostAsync<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(PostAsync));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(resourcePath, data);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        string msg =
            $"Post:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public T Put<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(Put));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        return PutAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    public async Task<T> PutAsync<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(Put));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.PutAsJsonAsync(resourcePath, data);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        string msg =
            $"Put:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public string Delete(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(Delete));

        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.DeleteAsync(resourcePath).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        string msg =
            $"Delete:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";

        throw new Exception(msg);
    }

    private HttpClient HttpClient(string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(HttpClient));

        HttpClient httpClient = _httpClientFactory.CreateClient(clientName);
        return httpClient;
    }
}