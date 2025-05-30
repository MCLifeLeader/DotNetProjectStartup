namespace Startup.Web.Services.Interfaces;

public interface IInfoService
{
    public string ReadApiCanaryPage();
    public Task<string> ReadApiCanaryPageAsync();
}