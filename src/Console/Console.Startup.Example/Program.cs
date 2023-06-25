using Microsoft.Extensions.Hosting;

namespace Console.Startup.Example;

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