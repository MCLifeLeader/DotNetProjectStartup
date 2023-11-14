using Microsoft.Extensions.Logging;
using Startup.Common.Connection.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace Startup.Common.Connection;

/// <summary>
/// Wrapper class for HttpClient that provides methods for making HTTP requests.
/// </summary>
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

    /// <summary>
    /// Sends a GET request to the specified resource path and returns the response body as a byte array.
    /// </summary>
    /// <param name="resourcePath">The resource path to send the GET request to.</param>
    /// <param name="clientName">The name of the HttpClient to use for the request.</param>
    /// <returns>The response body as a byte array.</returns>
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

    /// <summary>
    /// Sends a GET request to the specified Uri as an asynchronous operation and returns the response body as a byte array.
    /// </summary>
    /// <param name="resourcePath">The Uri the request is sent to.</param>
    /// <param name="clientName">The name of the HttpClient instance to use for the request.</param>
    /// <returns>A byte array containing the response body.</returns>
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

    /// <summary>
    /// Gets an object of type T from the specified resource path using the specified client name.
    /// </summary>
    /// <typeparam name="T">The type of object to get.</typeparam>
    /// <param name="resourcePath">The resource path to get the object from.</param>
    /// <param name="clientName">The name of the client to use for the request.</param>
    /// <returns>The object of type T retrieved from the specified resource path.</returns>
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
            return default!;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    /// <summary>
    /// Sends a GET request to the specified resource path using the specified client and returns the response as an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize the response content to.</typeparam>
    /// <param name="resourcePath">The resource path to send the GET request to.</param>
    /// <param name="clientName">The name of the HttpClient to use for the request.</param>
    /// <returns>The deserialized response content as an object of type T.</returns>
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
            return default!;
        }

        string msg = $"GET:{resourcePath} erred out with a result:{response.StatusCode}";
        throw new Exception(msg);
    }

    public string GetClientCredentialToken()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sends a GET request to a specified resource and retrieves an object of type T.
    /// </summary>
    /// <param name="resourcePath">The path to the resource you want to access.</param>
    /// <param name="clientName">Used to create an instance of HttpClient which is used to send the HTTP request.</param>
    /// <param name="token">The bearer token used for authorization.</param>
    /// <returns>
    /// If the request is successful (HTTP status code 200), it returns an object of type T that is the result of the GET request.
    /// If the status code of the response is 404 (Not Found), it returns the default value for type T.
    /// If the status code is anything other than 200 or 404, an exception is thrown with a message indicating the resource path and the status code.
    /// </returns>
    /// <exception cref="Exception">Thrown when the HTTP status code is anything other than 200 or 404.</exception>
    public T GetObjectUsingBearerToken<T>(string resourcePath, string clientName, string token)
    {
        // Log the method call and resource path
        _logger.LogDebug("'{Class}.{Method}<T>' called", GetType().Name, nameof(GetObjectUsingBearerToken));
        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);

        // Create an HttpClient instance
        HttpClient httpClient = HttpClient(clientName);
        HttpRequestMessage request = new();
        request.Headers.Add("Accept", "application/json");

        // Set the Authorization header to use the provided bearer token
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Set the HTTP method to GET and construct the request URI
        request.Method = HttpMethod.Get;
        string url = $"{httpClient.BaseAddress}{resourcePath}";
        request.RequestUri = new Uri(url);

        // Send the request and store the response
        HttpResponseMessage response = httpClient.SendAsync(request).Result;
        if (response.IsSuccessStatusCode)
        {
            // If the request is successful, read the content of the response and deserialize it into an object of type T
            Task<string> result = response.Content.ReadAsStringAsync();
            return response.Content.ReadAsAsync<T>().Result;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // If the status code is 404, return the default value for type T
            return default!;
        }

        // If the status code is anything other than 200 or 404, throw an exception
        string msg = $"GET:{resourcePath} erred out with a result: {response.StatusCode}";
        throw new Exception(msg);
    }

    /// <summary>
    /// Sends a POST request to the specified resource path with the given object data using the specified HTTP client.
    /// </summary>
    /// <param name="resourcePath">The resource path to send the POST request to.</param>
    /// <param name="o">The object data to include in the POST request.</param>
    /// <param name="clientName">The name of the HTTP client to use for the request.</param>
    /// <returns>The response content as a string if the request was successful.</returns>
    /// <exception cref="Exception">Thrown when the request fails with a non-success status code.</exception>
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

    /// <summary>
    /// Sends a PUT request to the specified resource path with the provided object as JSON payload using the specified HttpClient instance.
    /// </summary>
    /// <param name="resourcePath">The resource path to send the PUT request to.</param>
    /// <param name="o">The object to send as JSON payload.</param>
    /// <param name="clientName">The name of the HttpClient instance to use.</param>
    /// <returns>The response content as a string if the request was successful.</returns>
    /// <exception cref="Exception">Thrown when the request was unsuccessful.</exception>
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

    /// <summary>
    /// Sends a POST request to the specified resource path with the provided data using the specified HttpClient instance.
    /// </summary>
    /// <typeparam name="TK">The type of the data being sent in the request body.</typeparam>
    /// <typeparam name="T">The type of the expected response body.</typeparam>
    /// <param name="resourcePath">The path of the resource to send the request to.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="clientName">The name of the HttpClient instance to use for the request.</param>
    /// <returns>The deserialized response body.</returns>
    public T Post<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(Post));
        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);

        return PostAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    /// <summary>
    /// Sends a POST request to the specified resource path with the provided data using the specified HttpClient instance.
    /// </summary>
    /// <typeparam name="TK">The type of the data being sent in the request body.</typeparam>
    /// <typeparam name="T">The type of the expected response body.</typeparam>
    /// <param name="resourcePath">The path of the resource to send the request to.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="clientName">The name of the HttpClient instance to use for the request.</param>
    /// <returns>The deserialized response body.</returns>
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

    /// <summary>
    /// Sends a PUT request to the specified resource path with the provided data using the specified HttpClient instance.
    /// </summary>
    /// <typeparam name="TK">The type of the data being sent in the request body.</typeparam>
    /// <typeparam name="T">The type of the expected response body.</typeparam>
    /// <param name="resourcePath">The path of the resource to send the request to.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="clientName">The name of the HttpClient instance to use for the request.</param>
    /// <returns>The deserialized response body of type T.</returns>
    public T Put<TK, T>(string resourcePath, TK data, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}<TK, T>' called", GetType().Name, nameof(Put));
        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);

        return PutAsync<TK, T>(resourcePath, data, clientName).Result;
    }

    /// <summary>
    /// Sends a PUT request to the specified resource path with the provided data using the specified HttpClient instance.
    /// </summary>
    /// <typeparam name="TK">The type of the data being sent in the request body.</typeparam>
    /// <typeparam name="T">The type of the expected response body.</typeparam>
    /// <param name="resourcePath">The path of the resource to send the request to.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="clientName">The name of the HttpClient instance to use for the request.</param>
    /// <returns>The deserialized response body of type T.</returns>
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

        string msg = $"Put:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";
        throw new Exception(msg);
    }

    /// <summary>
    /// Sends a DELETE request to the specified resource path using the specified client name.
    /// </summary>
    /// <param name="resourcePath">The resource path to send the DELETE request to.</param>
    /// <param name="clientName">The name of the client to use for the request.</param>
    /// <returns>The response content as a string if the request was successful.</returns>
    /// <exception cref="Exception">Thrown when the request fails.</exception>
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

    /// <summary>
    /// Sends a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="resourcePath">The Uri the request is sent to.</param>
    /// <param name="clientName">The name of the HttpClient to use.</param>
    /// <returns>A Task that represents the asynchronous DELETE operation. The task result contains the HTTP response body as a string.</returns>
    public async Task<string> DeleteAsync(string resourcePath, string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(DeleteAsync));
        _logger.LogInformation("Request Resource Path:'{resourcePath}'", resourcePath);

        HttpClient httpClient = HttpClient(clientName);
        HttpResponseMessage response = await httpClient.DeleteAsync(resourcePath);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
        {
            return await response.Content.ReadAsStringAsync();
        }

        string msg =
            $"Delete:{resourcePath} erred out with a result:{response.StatusCode} and MsgResult:{response.Content.ReadAsStringAsync().Result}";

        throw new(msg);
    }

    /// <summary>
    /// Returns an instance of HttpClient for the specified client name.
    /// </summary>
    /// <param name="clientName">The name of the client to create.</param>
    /// <returns>An instance of HttpClient.</returns>
    public HttpClient HttpClient(string clientName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(HttpClient));

        HttpClient httpClient = _httpClientFactory.CreateClient(clientName);
        return httpClient;
    }
}