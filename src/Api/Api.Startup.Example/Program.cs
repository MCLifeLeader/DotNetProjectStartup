using Startup.Api;
using Startup.Api.Models.ApplicationSettings;

WebApplication.CreateBuilder(args)
    .RegisterServices(out AppSettings? appSettings)
    .Build()
    .SetupMiddleware(appSettings)
    .Run();