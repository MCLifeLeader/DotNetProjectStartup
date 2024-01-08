using Microsoft.Extensions.Compliance.Classification;

namespace Web.Startup.Example.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}