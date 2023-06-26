namespace Api.Startup.Example.Repositories.Http.Endpoints.Interfaces;

public interface ICanaryPageRepository
{
    public string GetCanaryPage();
    public string GetWeatherPage();
    Task<string> GetCanaryPageAsync();
    Task<string> GetWeatherPageAsync();
}