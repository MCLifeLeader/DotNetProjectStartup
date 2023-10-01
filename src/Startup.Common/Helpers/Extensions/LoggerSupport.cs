using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Startup.Common.Helpers.Extensions
{
    public static class LoggerSupport
    {
        /// <summary>
        /// Override the default logging factory to enable logging of SQL queries.
        /// </summary>
        /// <returns></returns>
        public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                    .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug)
                    .AddFilter(DbLoggerCategory.Update.Name, LogLevel.Debug);
            });

            return serviceCollection
                .BuildServiceProvider()
                .GetService<ILoggerFactory>();
        }
    }
}
