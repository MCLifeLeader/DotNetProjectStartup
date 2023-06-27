namespace Web.Startup.Example.Repository.Http.Endpoints.Interfaces;

public interface ICanaryPageCache
{
    bool SetCanaryPage(string key, string data);
    string GetCanaryPage(string key);
}