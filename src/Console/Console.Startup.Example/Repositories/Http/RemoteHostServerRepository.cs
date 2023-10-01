using System.Text;
using Console.Startup.Example.Repositories.Http.Interface;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;

namespace Console.Startup.Example.Repositories.Http;

public class RemoteHostServerRepository : IRemoteHostServerRepository
{
    private readonly IHttpClientWrapper _httpClient;

    public RemoteHostServerRepository(IHttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string?> GetFileFromServer(string urlPath, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        byte[] data = await _httpClient.GetBytesAsync(urlPath, HttpClientNames.STARTUP_WEB);
        return Encoding.UTF8.GetString(data);
    }
}