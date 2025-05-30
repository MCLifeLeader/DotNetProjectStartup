using Startup.Blazor.Server;
using Startup.Blazor.Server.Models.ApplicationSettings;

WebApplication.CreateBuilder(args)
    .RegisterServices(out AppSettings? appSettings)
    .Build()
    .SetupMiddleware(appSettings)
    .Run();