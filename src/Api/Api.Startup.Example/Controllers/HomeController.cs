using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Startup.Example.Controllers;

public class HomeController : BaseController
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/Error")]
    public IActionResult HandleError()
    {
        return new BadRequestObjectResult("Error");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/Error-Development")]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return new BadRequestObjectResult($"{exceptionHandlerFeature.Error.StackTrace} - {exceptionHandlerFeature.Error.Message}");
    }
}