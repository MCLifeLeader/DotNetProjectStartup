using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Startup.Web.Connection.DependencyInjection;
using Startup.Web.Data.DependencyInjection;
using Startup.Web.Factories.DependencyInjection;
using Startup.Web.Helpers.DependencyInjection;
using Startup.Web.Helpers.Extensions;
using Startup.Web.Helpers.Health;
using Startup.Web.Models.ApplicationSettings;
using Startup.Web.Services.DependencyInjection;
using Microsoft.FeatureManagement;
using Startup.Common.Constants;
using Microsoft.Extensions.Http.Resilience;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Startup.Common.Helpers.Data;
using Startup.Common.Helpers.Filter;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Startup.Web;


/// <summary>
/// Class responsible for registering dependent services in the application.
/// </summary>
public static class RegisterDependentServices
{
    private static AppSettings? _appSettings;

    /// <summary>
    /// Registers the dependent services in the application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="appSettings">The <see cref="AppSettings"/> instance.</param>
    /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder, out AppSettings? appSettings)
    {
        #region Configuration Setup

        // Pull from configuration files based on base or lane specific settings.
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

        // Pre-load developer secrets if present (Get local VaultUri)
        if (!builder.Environment.IsProduction())
        {
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
        }

        // Import Azure Key Vault Secrets and override any pre-loaded secrets
        string keyVaultUri = builder.Configuration.GetValue<string>("KeyVaultUri")!;
        if (!string.IsNullOrEmpty(keyVaultUri) && !keyVaultUri.Contains("Replace-Key", StringComparison.CurrentCultureIgnoreCase))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(builder.Configuration.GetValue<string>("KeyVaultUri")!),
                new DefaultAzureCredential());
        }

        // Import Environment Variables from the Host Server / Service
        builder.Configuration.AddEnvironmentVariables();

        // Import Developer Secrets and override any key vault values
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
        }

        builder.Services.Configure<AppSettings>(builder.Configuration);
        _appSettings = new()
        {
            ConfigurationBase = builder.Configuration
        };

        // Bind the app settings to the model
        builder.Configuration.Bind(_appSettings);

        // Adds the Fluent Validation to DI.
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

        // Validate the app settings model
        builder.Services
            .AddOptions<AppSettings>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateFluently()
            .ValidateOnStart();

        builder.Services.AddSingleton(builder.Configuration);
        builder.Services.AddSingleton(_appSettings);

        #endregion

        #region Logging Setup

        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        if (!_appSettings.ConnectionStrings.ApplicationInsights.Contains("Replace-Key", StringComparison.CurrentCultureIgnoreCase))
        {
            builder.Logging.AddApplicationInsights(
                configureApplicationInsightsLoggerOptions: (options) =>
                {
                    options.FlushOnDispose = true;
                });

            builder.Services.AddApplicationInsightsTelemetry(o =>
            {
                o.ConnectionString = _appSettings.ConnectionStrings.ApplicationInsights;
            });
        }

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging
                .AddConsole()
                .AddJsonConsole(o => o.JsonWriterOptions = new JsonWriterOptions
                {
                    Indented = true,
                    Encoder = JavaScriptEncoder.Default
                })
                .AddDebug();
        }

        if (_appSettings.FeatureManagement.OpenTelemetryEnabled)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(x =>
            {
                x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                    .AddService(Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown")
                    .AddAttributes(new Dictionary<string, object>()
                    {
                        ["deployment.environment"] = builder.Environment.EnvironmentName,
                        ["deployment.version"] = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0.0"
                    }));

                x.IncludeScopes = true;
                x.IncludeFormattedMessage = true;

                x.AddConsoleExporter();
                x.AddOtlpExporter(a =>
                {
                    a.Endpoint = new Uri(_appSettings.OpenTelemetry.Endpoint);
                    a.Protocol = OtlpExportProtocol.HttpProtobuf;
                    a.Headers = $"X-Seq-ApiKey={_appSettings.OpenTelemetry.ApiKey}";
                });
            });
        }

        builder.Services.AddHttpLogging(o =>
        {
            o.CombineLogs = true;
        });

        builder.Logging.EnableRedaction();
        builder.Services.AddRedaction(x =>
        {
            x.SetRedactor<ErasingRedactor>(new DataClassificationSet(DataTaxonomy.SensitiveData));
            x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PartialSensitiveData));

            x.SetHmacRedactor(o =>
            {
                o.Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.RedactionKey));
                o.KeyId = 1776;
            }, new DataClassificationSet(DataTaxonomy.Pii));

            x.SetFallbackRedactor<NullRedactor>();
            builder.Services.AddControllersWithViews(options =>
            {
                //options.Filters.Add<CustomExceptionFilterAttribute>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        });
        
        #endregion

        builder.Services.AddMemoryCache();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler(o =>
            {
                o.TotalRequestTimeout = new HttpTimeoutStrategyOptions()
                {
                    Name = "TotalTimeout",
                    Timeout = TimeSpan.FromSeconds(_appSettings.HttpClients.Resilience.BaseTimeOutInSeconds)
                };
                o.AttemptTimeout = new HttpTimeoutStrategyOptions()
                {
                    Name = "TotalTimeout",
                    Timeout = TimeSpan.FromSeconds(_appSettings.HttpClients.Resilience.BaseTimeOutInSeconds),
                };
                o.CircuitBreaker = new HttpCircuitBreakerStrategyOptions()
                {
                    Name = "TotalTimeout",
                    BreakDuration = TimeSpan.FromSeconds(_appSettings.HttpClients.Resilience.BaseTimeOutInSeconds),
                    SamplingDuration = TimeSpan.FromSeconds(_appSettings.HttpClients.Resilience.BaseTimeOutInSeconds * 2)
                };
            });
        });

        builder.SetHttpClients(_appSettings);

        builder.Services.AddFeatureManagement();

        builder.Services.AddRazorPages();

        builder.SetDependencyInjection(_appSettings);

        builder.Services.AddHealthChecks()
            .AddCheck<StartupExampleApiHealthCheck>(HttpClientNames.STARTUPEXAMPLE_API.ToLower())
            .AddCheck<OpenAiHealthCheck>(HttpClientNames.OPEN_AI_API_HEALTH);

        appSettings = _appSettings;
        return builder;
    }

    private static void SetDependencyInjection(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // connections
        ConnectionResolver.RegisterDependencies(builder.Services, appSettings);

        // services
        ServicesResolver.RegisterDependencies(builder.Services, appSettings);

        // helpers
        HelpersResolver.RegisterDependencies(builder.Services, appSettings);

        // repositories
        DataServicesResolver.RegisterDependencies(builder.Services, appSettings);

        //factories
        FactoriesResolver.RegisterDependencies(builder.Services, appSettings);
    }

    private static void SetHttpClients(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_API, c =>
        {
            c.BaseAddress = new Uri(appSettings.StartupExample.ApiUrl);

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

        builder.Services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_APP, c =>
        {
            c.BaseAddress = new Uri(appSettings.StartupExample.AppUrl);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            c.Timeout = TimeSpan.FromSeconds(120);
        }).ConfigurePrimaryHttpMessageHandler(c =>
        {
            HttpClientHandler h = new HttpClientHandler
            {
                UseProxy = false
            };
            return h;
        });

        builder.Services.AddHttpClient(HttpClientNames.OPEN_AI_API_HEALTH, c =>
        {
            c.BaseAddress = new Uri(appSettings.HealthCheckEndpoints.OpenAi);

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
    }
}
