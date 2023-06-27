using Blazor.Startup.Example;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();