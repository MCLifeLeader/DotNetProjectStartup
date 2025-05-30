using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Startup.Function.Api.Functions;

public class EndpointExample
{
    private readonly ILogger<EndpointExample> _logger;

    public EndpointExample(ILogger<EndpointExample> logger)
    {
        _logger = logger;
    }

    [Function("StartupGetExample")]
    public IActionResult StartupGetExample([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
    [Function("StartupPostExample")]
    public IActionResult StartupPostExample([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}