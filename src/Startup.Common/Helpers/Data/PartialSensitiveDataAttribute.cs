using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Common.Helpers.Data;

public class PartialSensitiveDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
    {

    }
}