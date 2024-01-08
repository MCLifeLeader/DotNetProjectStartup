using Microsoft.Extensions.Compliance.Classification;

namespace Console.Startup.Example.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}