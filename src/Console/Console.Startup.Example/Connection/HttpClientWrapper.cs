using System.Net;
using Console.Startup.Example.Connection.Interfaces;

namespace Console.Startup.Example.Connection;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientWrapper(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public byte[] GetBytes(string resourcePath, string clientName)
    {
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
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.GetAsync(resourcePath).Result;
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public T GetObject<T>(string resourcePath, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.GetAsync(resourcePath).Result;
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsAsync<T>().Result;
        }

        if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
        {
            return default!;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public async Task<T> GetObjectAsync<T>(string resourcePath, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.GetAsync(resourcePath);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
        {
            return default!;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public string PostData(string resourcePath, object o, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.PostAsJsonAsync(resourcePath, o).Result;
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        string msg =
            $"Post:{resourcePath} errored out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public string PutData(string resourcePath, object o, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = httpClient.PutAsJsonAsync(resourcePath, o).Result;
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
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
        return PostAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    public async Task<T> PostAsync<TK, T>(string resourcePath, TK data, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(resourcePath, data);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        string msg =
            $"Post:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public T Put<TK, T>(string resourcePath, TK data, string clientName)
    {
        return PutAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    public async Task<T> PutAsync<TK, T>(string resourcePath, TK data, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.PutAsJsonAsync(resourcePath, data);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
        {
            return await response.Content.ReadAsAsync<T>();
        }

        string msg =
            $"Put:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    public async Task<string> DeleteAsync(string resourcePath, string clientName)
    {
        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.DeleteAsync(resourcePath);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
        {
            return await response.Content.ReadAsStringAsync();
        }

        string msg =
            $"Delete:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";

        throw new Exception(msg);
    }

    public string Delete(string resourcePath, string clientName)
    {
        return DeleteAsync(resourcePath, clientName).Result;
    }

    private HttpClient HttpClient(string clientName)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(clientName);
        return httpClient;
    }
}