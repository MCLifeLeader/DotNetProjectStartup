using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using Web.Startup.Example.Connection.DependencyInjection;
using Web.Startup.Example.Constants;
using Web.Startup.Example.Data.DependencyInjection;
using Web.Startup.Example.Factories.DependencyInjection;
using Web.Startup.Example.Helpers.DependencyInjection;
using Web.Startup.Example.Helpers.Extensions;
using Web.Startup.Example.Helpers.Health;
using Web.Startup.Example.Models.ApplicationSettings;
using Web.Startup.Example.Services.DependencyInjection;

namespace Web.Startup.Example;

/// <summary>
/// 
/// </summary>
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
        if (!string.IsNullOrEmpty(builder.Configuration.GetValue<string>("KeyVaultUri")))
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
                .AddJsonConsole(o => o.JsonWriterOptions = new JsonWriterOptions { Indented = true })
                .AddDebug();
        }

        builder.Services.AddHttpLogging(o =>
        {
            o.CombineLogs = true;
        });

        builder.Services.AddMemoryCache();
        builder.SetHttpClients(appSettings);

        builder.Services.AddRazorPages();
        builder.SetDependencyInjection(appSettings);

        builder.Services.AddHealthChecks()
            .AddCheck<StartupExampleAppHealthCheck>(HttpClientNames.STARTUPEXAMPLE_API.ToLower());

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
    }
}