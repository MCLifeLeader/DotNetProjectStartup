using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Api.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}