using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Web.Helpers.Data;

public class PiiDataAttribute : DataClassificationAttribute
{
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {

    }
}