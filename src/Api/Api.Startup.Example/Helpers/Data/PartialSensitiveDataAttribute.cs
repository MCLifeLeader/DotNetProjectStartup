using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Api.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}