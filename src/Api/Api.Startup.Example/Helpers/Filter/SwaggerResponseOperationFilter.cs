using System.Net;
using Api.Startup.Example.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Startup.Example.Helpers.Filter;

public class SwaggerResponseOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the specified operation. Adds 500 ServerError to Swagger documentation for all endpoints
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // ensure we are filtering on controllers
        if (context?.MethodInfo?.DeclaringType?.BaseType?.BaseType == typeof(ControllerBase) ||
            context?.MethodInfo?.ReflectedType?.BaseType == typeof(Controller))
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            // Allow override of response codes by checking for existing status code key
            if (!operation.Responses.ContainsKey($"{(int) statusCode}"))
            {
                operation.Responses.Add($"{(int) statusCode}", new OpenApiResponse
                {
                    Description = $"{statusCode} - See Error Results for Details",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json", new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResult), context.SchemaRepository)
                            }
                        }
                    }
                });
            }

            statusCode = HttpStatusCode.BadRequest;
            // Allow override of response codes by checking for existing status code key
            if (!operation.Responses.ContainsKey($"{(int) statusCode}"))
            {
                operation.Responses.Add(((int) statusCode).ToString(), new OpenApiResponse
                {
                    Description = $"{statusCode} - See Error Results for Details",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json", new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResult), context.SchemaRepository)
                            }
                        }
                    }
                });
            }

            statusCode = HttpStatusCode.NotFound;
            // Allow override of response codes by checking for existing status code key
            if (!operation.Responses.ContainsKey($"{(int) statusCode}"))
            {
                operation.Responses.Add(((int) statusCode).ToString(), new OpenApiResponse
                {
                    Description = $"{statusCode} - See Error Results for Details",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json", new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResult), context.SchemaRepository)
                            }
                        }
                    }
                });
            }

            statusCode = HttpStatusCode.Unauthorized;
            // Allow override of response codes by checking for existing status code key
            if (!operation.Responses.ContainsKey($"{(int) statusCode}"))
            {
                operation.Responses.Add(((int) statusCode).ToString(), new OpenApiResponse
                {
                    Description = $"{statusCode}",
                });
            }
        }
    }
}