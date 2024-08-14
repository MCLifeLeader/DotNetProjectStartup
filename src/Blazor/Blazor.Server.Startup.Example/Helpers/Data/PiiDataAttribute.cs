using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Blazor.Server.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}