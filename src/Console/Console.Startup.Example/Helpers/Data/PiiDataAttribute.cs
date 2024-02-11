using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Console.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}