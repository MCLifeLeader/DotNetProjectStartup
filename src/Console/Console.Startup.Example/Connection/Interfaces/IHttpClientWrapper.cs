namespace Console.Startup.Example.Connection.Interfaces;

public interface IHttpClientWrapper
{
    T GetObject<T>(string resourcePath, string clientName);
    Task<T> GetObjectAsync<T>(string resourcePath, string clientName);
    byte[] GetBytes(string resourcePath, string clientName);
    Task<byte[]> GetBytesAsync(string resourcePath, string clientName);
    string PostData(string resourcePath, object o, string clientName);
    string PutData(string resourcePath, object o, string clientName);
    Task<string> DeleteAsync(string resourcePath, string clientName);
    string Delete(string resourcePath, string clientName);
    T Post<TK, T>(string resourcePath, TK data, string clientName);
    Task<T> PostAsync<TK, T>(string resourcePath, TK data, string clientName);
    T Put<TK, T>(string resourcePath, TK data, string clientName);
    Task<T> PutAsync<TK, T>(string resourcePath, TK data, string clientName);
}