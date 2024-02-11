namespace Startup.Console.Repositories.Http.Interface;

public interface IRemoteHostServerRepository
{
    Task<string?> GetFileFromServer(string urlPath, CancellationToken cancellationToken);
}