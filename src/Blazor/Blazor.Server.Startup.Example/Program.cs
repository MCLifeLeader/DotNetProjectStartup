using Startup.Blazor.Server;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();