using Microsoft.Extensions.Hosting;

namespace Startup.Console;

public static class SetupMiddlewarePipeline
{
    public static IHost SetupMiddleware(this IHost app)
    {
        return app;
    }
}