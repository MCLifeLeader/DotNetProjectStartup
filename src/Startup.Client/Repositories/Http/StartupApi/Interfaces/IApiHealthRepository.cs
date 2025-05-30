namespace Startup.Client.Repositories.Http.StartupApi.Interfaces;

public interface IApiHealthRepository
{
    /// <summary>
    /// The health API endpoint is not published in the OpenAPI specification.
    /// </summary>
    /// <returns></returns>
    Task<string> GetHealthAsync();
}