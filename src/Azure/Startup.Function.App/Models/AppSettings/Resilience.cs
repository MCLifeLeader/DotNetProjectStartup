using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public class Resilience
{
    public int BaseTimeOutInSeconds { get; set; }
}