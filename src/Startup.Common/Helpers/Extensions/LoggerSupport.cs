using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using System.Reflection;

namespace Startup.Common.Helpers.Extensions;

public static class LoggerSupport
{
    /// <summary>
    /// Override the default logging factory to enable logging of SQL queries.
    /// </summary>
    /// <returns></returns>

    public static ILoggerFactory GetLoggerFactory(IServiceCollection? roServiceCollection)
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        var serviceProvider = roServiceCollection?.BuildServiceProvider();
        var configuration = serviceProvider?.GetService<IConfiguration>();
        var openTelemetryEnabled = configuration != null && configuration.GetValue<bool>("FeatureManagement:OpenTelemetryEnabled");

        if (openTelemetryEnabled)
        {
            string endpoint = configuration?.GetValue<string>("OpenTelemetry:Endpoint") ?? string.Empty;
            string apiKey = configuration?.GetValue<string>("OpenTelemetry:ApiKey") ?? string.Empty;

            serviceCollection.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(x =>
                    {
                        x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                            .AddService(Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown")
                            .AddAttributes(new Dictionary<string, object>()
                            {
                                ["service.execution"] = "sql",
                                ["deployment.environment"] = configuration?.GetValue<string>("Environment") ?? "Unknown",
                                ["deployment.version"] = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0.0"
                            }));

                        x.IncludeScopes = true;
                        x.IncludeFormattedMessage = true;

                        x.AddConsoleExporter();
                        x.AddOtlpExporter(a =>
                        {
                            a.Endpoint = new Uri(endpoint);
                            a.Protocol = OtlpExportProtocol.HttpProtobuf;
                            a.Headers = $"X-Seq-ApiKey={apiKey}";
                        });
                    })
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                    .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug)
                    .AddFilter(DbLoggerCategory.Update.Name, LogLevel.Debug);
            });
        }
        else
        {
            // Fallback to default logging if no ServiceCollection is provided.
            if (roServiceCollection == null)
            {
                serviceCollection.AddLogging(builder =>
                {
                    builder
                        .AddDebug()
                        .AddConsole()
                        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                        .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug)
                        .AddFilter(DbLoggerCategory.Update.Name, LogLevel.Debug);
                });
            }
        }

        return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>()!;
    }
}
