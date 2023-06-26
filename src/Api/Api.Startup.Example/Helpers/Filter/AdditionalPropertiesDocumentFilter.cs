using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Startup.Example.Helpers.Filter;

/// <summary>
/// Newtonsoft Document Filter to assist the function of the Autorest generator
/// </summary>
public class AdditionalPropertiesDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="openApiDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        foreach (KeyValuePair<string, OpenApiSchema> schema in context.SchemaRepository.Schemas
                     .Where(schema => schema.Value.AdditionalProperties == null))
            schema.Value.AdditionalPropertiesAllowed = true;
    }
}