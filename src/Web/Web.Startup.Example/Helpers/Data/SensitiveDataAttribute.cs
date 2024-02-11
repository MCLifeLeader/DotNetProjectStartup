using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Web.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}