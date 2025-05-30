using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Startup.Common.Helpers.Extensions;

/// <summary>
/// Performance-oriented helper methods for collections.
/// Provides extension methods for <see cref="Dictionary{TKey, TValue}"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to operate on.</param>
    /// <param name="key">The key whose value to get or add.</param>
    /// <param name="value">The value to add if the key does not exist.</param>
    /// <returns>The value associated with the specified key, or the newly added value.</returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        if (dict.TryGetValue(key, out TValue? existingValue))
        {
            return existingValue;
        }

        dict[key] = value;
        return value;
    }

    /// <summary>
    /// Tries to update the value associated with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to operate on.</param>
    /// <param name="key">The key whose value to update.</param>
    /// <param name="value">The new value to set for the specified key.</param>
    /// <returns><c>true</c> if the value was updated; otherwise, <c>false</c>.</returns>
    public static bool TryUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
    {
        // Note: Use unsafely-casting to silence nullable warning for value types
        ref TValue val = ref CollectionsMarshal.GetValueRefOrNullRef(dictionary, key);
        if (Unsafe.IsNullRef(ref val))
        {
            return false;
        }

        val = value;
        return true;
    }
}