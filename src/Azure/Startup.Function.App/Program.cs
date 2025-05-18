using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Startup.Common.Constants;
using Startup.Common.Helpers.Data;
using Startup.Common.Helpers.Filter;
using Startup.Data.Repositories.DependencyInjection;
using Startup.Function.Api.DependencyRegistration;
using Startup.Function.Api.Helpers.Extensions;
using Startup.Function.Api.Helpers.Health;
using Startup.Function.Api.Models.AppSettings;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

//var builder = FunctionsApplication.CreateBuilder(args);

//builder.ConfigureFunctionsWebApplication();

//// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//// builder.Services
////     .AddApplicationInsightsTelemetryWorkerService()
////     .ConfigureFunctionsApplicationInsights();

//builder.Build().Run();

namespace Startup.Function.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        AppSettings appSettings = new();

        IHost host = new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                #region Setup Configuration
                config.AddJsonFile("appsettings.json", true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);

                if (!context.HostingEnvironment.IsProduction())
                {
                    config.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
                }

                // Import Environment Variables from the Host Server / Service
                config.AddEnvironmentVariables();

                #endregion
            })
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((context, services) =>
            {
                #region Bind AppSettings
                services.Configure<AppSettings>(context.Configuration);
                appSettings.ConfigurationBase = context.Configuration;

                // Bind the app settings to the model
                context.Configuration.Bind(appSettings);

                // Adds the Fluent Validation to DI.
                services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

                services.AddOptions<AppSettings>()
                    .Bind(context.Configuration)
                    .ValidateDataAnnotations()
                    .ValidateFluently()
                    .ValidateOnStart();

                services.AddSingleton(context.Configuration);
                services.AddSingleton(appSettings);
                #endregion

                services.AddFeatureManagement();

                #region Logging and Telemetry
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddRedaction(x =>
                {
                    x.SetRedactor<ErasingRedactor>(new DataClassificationSet(DataTaxonomy.SensitiveData));
                    x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PartialSensitiveData));
                    x.SetHmacRedactor(o =>
                    {
                        o.Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(appSettings.RedactionKey!));
                        o.KeyId = 1830;
                    }, new DataClassificationSet(DataTaxonomy.Pii));
                    x.SetFallbackRedactor<NullRedactor>();
                });

                #endregion

                #region Http Services
                services.AddRequestDecompression();
                services.AddResponseCompression(options => { options.EnableForHttps = true; });

                #region HttpClient Services
                services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_API, c =>
                {
                    c.BaseAddress = new Uri(appSettings.HttpClients!.StartupExample.BaseUrl!);

                    c.DefaultRequestHeaders.Accept.Clear();
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    c.Timeout = TimeSpan.FromSeconds(120);
                }).ConfigurePrimaryHttpMessageHandler(c =>
                {
                    HttpClientHandler h = new HttpClientHandler
                    {
                        UseProxy = false
                    };
                    return h;
                });
                #endregion

                services.ConfigureHttpClientDefaults(http =>
                {
                    // Turn on resilience by default
                    http.AddStandardResilienceHandler(o =>
                    {
                        o.TotalRequestTimeout = new HttpTimeoutStrategyOptions()
                        {
                            Name = "TotalTimeout",
                            Timeout = TimeSpan.FromSeconds(appSettings.HttpClients!.Resilience!.BaseTimeOutInSeconds)
                        };
                        o.AttemptTimeout = new HttpTimeoutStrategyOptions()
                        {
                            Name = "TotalTimeout",
                            Timeout = TimeSpan.FromSeconds(appSettings.HttpClients.Resilience.BaseTimeOutInSeconds),
                        };
                        o.CircuitBreaker = new HttpCircuitBreakerStrategyOptions()
                        {
                            Name = "TotalTimeout",
                            BreakDuration = TimeSpan.FromSeconds(appSettings.HttpClients.Resilience.BaseTimeOutInSeconds),
                            SamplingDuration = TimeSpan.FromSeconds(appSettings.HttpClients.Resilience.BaseTimeOutInSeconds * 2)
                        };
                    });
                });

                services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
                #endregion

                services.AddHealthChecks()
                    .AddCheck<StartupExampleApiHealthCheck>(nameof(StartupExampleApiHealthCheck))
                    .AddSqlServer(appSettings.ConnectionStrings!.DefaultConnection);

                DependencyResolution.RegisterDependencies(services, context, appSettings);
                RepositoriesResolver.RegisterDependencies(services, appSettings.ConnectionStrings.DefaultConnection!);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                logging.AddJsonConsole(o => o.JsonWriterOptions = new JsonWriterOptions
                {
                    Indented = true,
                    Encoder = JavaScriptEncoder.Default
                });

                logging.EnableRedaction();

                #region Developer Logging and Telemetry
                if (context.HostingEnvironment.IsDevelopment() || context.HostingEnvironment.IsStaging())
                {
                    logging
                        .AddConsole()
                        .AddJsonConsole(o => o.JsonWriterOptions = new JsonWriterOptions
                        {
                            Indented = true,
                            Encoder = JavaScriptEncoder.Default
                        })
                        .AddDebug();
                }

                if (appSettings.FeatureManagement!.OpenTelemetryEnabled!)
                {
                    logging.AddOpenTelemetry(x =>
                    {
                        x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                            .AddService(Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown")
                            .AddAttributes(new Dictionary<string, object>()
                            {
                                ["deployment.environment"] = context.HostingEnvironment.EnvironmentName,
                                ["deployment.version"] = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0.0"
                            }));

                        x.IncludeScopes = true;
                        x.IncludeFormattedMessage = true;

                        x.AddConsoleExporter();
                        x.AddOtlpExporter(a =>
                        {
                            a.Endpoint = new Uri(appSettings.OpenTelemetry!.Endpoint!);
                            a.Protocol = OtlpExportProtocol.HttpProtobuf;
                            a.Headers = $"X-Seq-ApiKey={appSettings.OpenTelemetry.ApiKey}";
                        });
                    });
                }
                #endregion
            })
            .Build();

        await host.RunAsync();
    }
}