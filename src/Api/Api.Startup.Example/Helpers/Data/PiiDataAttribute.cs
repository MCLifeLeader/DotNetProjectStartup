using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Api.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}