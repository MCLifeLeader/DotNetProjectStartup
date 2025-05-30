using Microsoft.Extensions.Hosting;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .RegisterServices(out AppSettings? appSettings)
            .Build()
            .SetupMiddleware(appSettings)
            .RunAsync();

        return Environment.ExitCode;
    }
}