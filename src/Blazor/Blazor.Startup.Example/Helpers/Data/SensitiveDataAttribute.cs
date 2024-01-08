using Microsoft.Extensions.Compliance.Classification;

namespace Blazor.Startup.Example.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}