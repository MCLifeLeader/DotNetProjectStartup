using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Common.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}