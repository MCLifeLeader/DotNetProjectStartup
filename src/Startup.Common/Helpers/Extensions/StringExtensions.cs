namespace Startup.Common.Helpers.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Replaces double slashes in a URL with a single slash, except for 'http://' or 'https://'.
    /// </summary>
    /// <param name="url">The URL string to be scrubbed.</param>
    /// <returns>The scrubbed URL string.</returns>
    public static string ScrubUrlRoute(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return url;
        }

        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            var protocol = url.StartsWith("http://") ? "http://" : "https://";
            var restOfUrl = url.Substring(protocol.Length);
            return protocol + restOfUrl.Replace("//", "/");
        }

        return url.Replace("//", "/");
    }

    /// <summary>
    /// Masks a string by replacing characters after the first four with asterisks.
    /// </summary>
    /// <param name="s">The string to be masked.</param>
    /// <returns>The masked string.</returns>
    public static string Mask(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return s;
        }

        if (s.Length <= 4)
        {
            return "****";
        }

        return $"{s.Substring(0, 4)}********";
    }
}