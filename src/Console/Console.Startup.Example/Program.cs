using Microsoft.Extensions.Hosting;

namespace Startup.Console;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .RegisterServices()
            .Build()
            .SetupMiddleware()
            .RunAsync();

        return Environment.ExitCode;
    }
}