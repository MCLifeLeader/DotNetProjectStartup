namespace Web.Startup.Example.Services.Interfaces;

public interface ICanaryService
{
    public string ReadApiCanaryPage();
    public Task<string> ReadApiCanaryPageAsync();
}