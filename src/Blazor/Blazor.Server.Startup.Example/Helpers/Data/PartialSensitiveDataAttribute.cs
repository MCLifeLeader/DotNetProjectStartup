using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Blazor.Server.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        // ReSharper disable once ConvertToPrimaryConstructor
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}