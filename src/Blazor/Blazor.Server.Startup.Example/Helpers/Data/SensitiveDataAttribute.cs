using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Blazor.Server.Helpers.Data;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public SensitiveDataAttribute() : base(DataTaxonomy.SensitiveData)
    {

    }
}