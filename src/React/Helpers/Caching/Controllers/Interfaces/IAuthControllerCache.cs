namespace React.Startup.Example.Helpers.Caching.Controllers.Interfaces;

public interface IAuthControllerCache
{
    bool SetAuth(string key, string authToken);
    string GetAuth(string key);
}