using System.Text;
using Console.Startup.Example.Connection.Interfaces;
using Console.Startup.Example.Constants;
using Console.Startup.Example.Repositories.Http.Interface;

namespace Console.Startup.Example.Repositories.Http;

public class RemoteHostServerRepository : IRemoteHostServerRepository
{
    private readonly IHttpClientWrapper _httpClient;

    public RemoteHostServerRepository(IHttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> GetMainHostsFileFromServer(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        string url = "hosts.main.txt";

        var data = await _httpClient.GetBytesAsync(url, HttpClientNames.RemoteHostServerClient);

        string result = null;
        if (data != null)
        {
            result = Encoding.UTF8.GetString(data);
        }

        return result;
    }

    public async Task<string> GetAltHostsFileFromServer(string machineName, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        string url = $"hosts.{machineName}.txt";

        var data = await _httpClient.GetBytesAsync(url, HttpClientNames.RemoteHostServerClient);

        string result = null;
        if (data != null)
        {
            result = Encoding.UTF8.GetString(data);
        }

        return result;
    }
}