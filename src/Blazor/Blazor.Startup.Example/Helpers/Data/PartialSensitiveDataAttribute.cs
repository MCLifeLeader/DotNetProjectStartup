using Microsoft.Extensions.Compliance.Classification;

namespace Blazor.Startup.Example.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}