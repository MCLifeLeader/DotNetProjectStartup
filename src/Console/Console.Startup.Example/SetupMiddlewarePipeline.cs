using Microsoft.Extensions.Hosting;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console;

public static class SetupMiddlewarePipeline
{
    public static IHost SetupMiddleware(this IHost app, AppSettings? appSettings)
    {
        return app;
    }
}