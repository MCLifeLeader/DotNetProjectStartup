namespace Edu.Si.Project.Server.Connection.Interfaces
{
    public interface IAbstractedCache : IDisposable
    {
        bool TryGetValue<T>(string key, out T data);
        void Set<T>(string key, T data, TimeSpan ts);
    }
}
