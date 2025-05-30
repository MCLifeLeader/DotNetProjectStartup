using System.Diagnostics.CodeAnalysis;

namespace Startup.Common.Constants;

/// <summary>
/// Contains logging templates used throughout the project.
/// </summary>
[ExcludeFromCodeCoverage]
public class LoggingTemplates
{
    #region Base Templates

    /// <summary>
    /// Template for logging method entry debug messages.
    /// </summary>
    public static string DebugMethodEntryMessage = "'{Class}.{Method}' called";

    #endregion

    #region ClientWrapper

    /// <summary>
    /// Template for logging standard HTTP resource request messages.
    /// </summary>
    public static string InfoHttpResourceStandardMessage = "Request Resource Path:'{resourcePath}'";

    #endregion
}