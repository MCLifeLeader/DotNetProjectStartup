using Microsoft.Extensions.Compliance.Classification;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Common.Helpers.Data;

[ExcludeFromCodeCoverage]
public class PartialSensitiveDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
    {

    }
}