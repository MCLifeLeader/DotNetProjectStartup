using Api.Startup.Example;
using Microsoft.Extensions.Azure;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();