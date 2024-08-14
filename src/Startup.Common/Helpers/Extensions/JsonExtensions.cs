using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Startup.Common.Helpers.Extensions;

/// <summary>
/// Json object manipulation extension methods
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    ///     Async Parse an object from a string using the object Template return type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static async Task<T> FromJsonAsync<T>(this string json)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <summary>
    ///     Parse an object from a string using the object Template return type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    ///     Async Convert an object into a json string object
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task<string> ToJsonAsync(this object value)
    {
        return await Task.Run(() => JsonConvert.SerializeObject(value));
    }

    /// <summary>
    ///     Convert an object into a json string object
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? ToJson(this object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    /// <summary>
    ///     Async Convert a json string to an object of type that is unknown
    /// </summary>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    public static async Task<JObject?> FromJsonToJObjectAsync(this string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return await Task.Run(() => JObject.Parse(json));
    }

    /// <summary>
    ///     Convert a json string to an object of type that is unknown
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JObject FromJsonToJObject(this string json)
    {
        return JObject.Parse(json);
    }

    /// <summary>
    ///     Async Convert a json array string to an object of type that is unknown
    /// </summary>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    public static async Task<JArray?> FromJsonToJArrayAsync(this string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return await Task.Run(() => JArray.Parse(json));
    }

    /// <summary>
    ///     Convert a json array string to an object of type that is unknown
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JArray FromJsonToJArray(this string json)
    {
        return JArray.Parse(json);
    }
}