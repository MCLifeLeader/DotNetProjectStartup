using Api.Startup.Example;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();

namespace Api.Startup.Example
{
}