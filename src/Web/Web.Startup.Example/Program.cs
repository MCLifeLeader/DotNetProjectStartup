using Startup.Web;
using Startup.Web.Models.ApplicationSettings;

WebApplication.CreateBuilder(args)
    .RegisterServices(out AppSettings? appSettings)
    .Build()
    .SetupMiddleware(appSettings)
    .Run();