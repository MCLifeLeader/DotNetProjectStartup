namespace Console.Startup.Example.Repositories.Http.Interface;

public interface IRemoteHostServerRepository
{
    Task<string> GetMainHostsFileFromServer(CancellationToken cancellationToken);
    Task<string> GetAltHostsFileFromServer(string machineName, CancellationToken cancellationToken);
}