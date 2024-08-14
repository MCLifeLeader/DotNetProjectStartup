using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Api.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}