using Azure.Identity;
using Startup.Blazor.Server.Connection.DependencyInjection;
using Startup.Blazor.Server.Data.DependencyInjection;
using Startup.Blazor.Server.Factories.DependencyInjection;
using Startup.Blazor.Server.Helpers.Data;
using Startup.Blazor.Server.Helpers.DependencyInjection;
using Startup.Blazor.Server.Helpers.Extensions;
using Startup.Blazor.Server.Helpers.Filter;
using Startup.Blazor.Server.Helpers.Health;
using Startup.Blazor.Server.Models.ApplicationSettings;
using Startup.Blazor.Server.Services.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Startup.Common.Constants;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using OpenTelemetry.Logs;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;

namespace Startup.Blazor.Server;

public static class RegisterDependentServices
{
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
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
        if (!string.IsNullOrEmpty(keyVaultUri) && !keyVaultUri.ToLower().Contains("na"))
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
        AppSettings appSettings = new()
        {
            ConfigurationBase = builder.Configuration
        };

        // Bind the app settings to the model
        builder.Configuration.Bind(appSettings);

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

        #endregion

        // Configure logging 
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        builder.Logging.EnableRedaction();

        if (!appSettings.ConnectionStrings.ApplicationInsights.ToLower().Contains("na"))
        {
            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                    config.ConnectionString = appSettings.ConnectionStrings.ApplicationInsights,
                configureApplicationInsightsLoggerOptions: (options) => { });
        }

        // EventLog is only available in a Windows environment
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //ToDo: Consider removing windows Event Logging in favor of just logging to App Insights
            builder.Logging.AddEventLog();
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

        if (appSettings.FeatureManagement.OpenTelemetryEnabled)
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
                    a.Endpoint = new Uri(appSettings.OpenTelemetry.Endpoint);
                    a.Protocol = OtlpExportProtocol.HttpProtobuf;
                    a.Headers = $"X-Seq-ApiKey={appSettings.OpenTelemetry.ApiKey}";
                });
            });
        }

        builder.Services.AddHttpLogging(o =>
        {
            o.CombineLogs = true;
        });

        builder.Services.AddRedaction(x =>
        {
            x.SetRedactor<ErasingRedactor>(new DataClassificationSet(DataTaxonomy.SensitiveData));

            x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PartialSensitiveData));

#pragma warning disable EXTEXP0002 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            x.SetHmacRedactor(o =>
            {
                o.Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(appSettings.RedactionKey));
                o.KeyId = 1776;
            }, new DataClassificationSet(DataTaxonomy.Pii));
#pragma warning restore EXTEXP0002 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        });

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();
        });

        builder.SetHttpClients(appSettings);
        builder.Services.AddMemoryCache();

        builder.SetDependencyInjection(appSettings);

        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        builder.Services.AddHealthChecks()
            .AddCheck<StartupExampleAppHealthCheck>(HttpClientNames.STARTUPEXAMPLE_API.ToLower())
            .AddCheck<OpenAiHealthCheck>(HttpClientNames.OPEN_AI_API_HEALTH);

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