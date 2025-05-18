using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Constants;

[ExcludeFromCodeCoverage]
public class FeatureFlags : Common.Constants.FeatureFlags
{
    public const string OPEN_API_ENABLED = "OpenApiEnabled";
}