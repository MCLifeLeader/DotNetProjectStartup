namespace Startup.Common.Helpers.Extensions;

public static class StringExtensions
{
    public static string ScrubUrlRoute(this string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return url;
        }

        return url.Replace("//", "/");
    }
    public static string Mask(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return s;
        };
        if (s.Length <= 4)
        {
            return "****";
        }
        return $"{s.Substring(0, 4)}********";
    }
}