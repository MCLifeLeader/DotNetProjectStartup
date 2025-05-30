using Startup.Common.Helpers.Data;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.Models.AppSettings;

[ExcludeFromCodeCoverage]
public record ConnectionStrings
{
    [SensitiveData]
    public string DefaultConnection { get; set; } = default!;
}