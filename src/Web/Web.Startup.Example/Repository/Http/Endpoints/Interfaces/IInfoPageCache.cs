namespace Startup.Web.Repository.Http.Endpoints.Interfaces;

public interface IInfoPageCache
{
    bool SetCanaryPage(string key, string? data);
    string? GetCanaryPage(string key);
}