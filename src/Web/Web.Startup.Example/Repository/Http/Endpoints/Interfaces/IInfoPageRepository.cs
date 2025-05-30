namespace Startup.Web.Repository.Http.Endpoints.Interfaces;

public interface IInfoPageRepository
{
    public string? GetCanaryPage();
    public string GetWeatherPage();
    Task<string?> GetCanaryPageAsync();
    Task<string> GetWeatherPageAsync();
}