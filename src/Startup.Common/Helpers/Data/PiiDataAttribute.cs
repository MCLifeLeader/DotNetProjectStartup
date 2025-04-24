using Microsoft.Extensions.Compliance.Classification;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Common.Helpers.Data;

[ExcludeFromCodeCoverage]
public class PiiDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}