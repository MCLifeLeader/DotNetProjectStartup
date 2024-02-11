namespace Startup.Blazor.Server.Services.Interfaces;

public interface IInfoService
{
    public string ReadApiCanaryPage();
    public Task<string> ReadApiCanaryPageAsync();
}