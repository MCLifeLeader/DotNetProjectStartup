using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Api.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        // ReSharper disable once ConvertToPrimaryConstructor
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}