using Microsoft.Extensions.Compliance.Classification;

namespace Startup.Web.Helpers.Data
{
    public class PartialSensitiveDataAttribute : DataClassificationAttribute
    {
        public PartialSensitiveDataAttribute() : base(DataTaxonomy.PartialSensitiveData)
        {

        }
    }
}