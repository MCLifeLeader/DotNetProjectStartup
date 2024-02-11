using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Console.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}