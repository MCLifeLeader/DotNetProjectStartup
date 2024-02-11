namespace Startup.Console.Model.ApplicationSettings;

public class Workerprocesses
{
    public int SleepDelaySeconds { get; set; }
    public Startupapi StartupApi { get; set; }
    public Remoteserverconnection RemoteServerConnection { get; set; }
}