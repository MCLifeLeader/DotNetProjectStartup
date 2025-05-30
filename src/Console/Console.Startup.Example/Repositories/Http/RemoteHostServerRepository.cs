using Startup.Common.Connection.Interfaces;
using Startup.Common.Constants;
using Startup.Console.Repositories.Http.Interface;
using System.Text;

namespace Startup.Console.Repositories.Http;

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

        byte[] data = await _httpClient.GetBytesAsync(urlPath, HttpClientNames.STARTUPEXAMPLE_HOME);
        return Encoding.UTF8.GetString(data);
    }
}