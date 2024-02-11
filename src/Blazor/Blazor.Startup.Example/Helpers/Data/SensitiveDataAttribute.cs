using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Blazor.Server.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}