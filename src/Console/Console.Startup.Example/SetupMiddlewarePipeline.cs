using Microsoft.Extensions.Hosting;

namespace Console.Startup.Example;

public static class SetupMiddlewarePipeline
{
    public static IHost SetupMiddleware(this IHost app)
    {
        return app;
    }
}