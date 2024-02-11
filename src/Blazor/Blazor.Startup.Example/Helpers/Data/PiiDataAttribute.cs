using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Blazor.Server.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}