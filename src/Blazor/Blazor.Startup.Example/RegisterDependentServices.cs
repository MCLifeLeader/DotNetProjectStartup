using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using Azure.Identity;
using Blazor.Startup.Example.Connection.DependencyInjection;
using Blazor.Startup.Example.Constants;
using Blazor.Startup.Example.Data.DependencyInjection;
using Blazor.Startup.Example.Factories.DependencyInjection;
using Blazor.Startup.Example.Helpers.DependencyInjection;
using Blazor.Startup.Example.Helpers.Extensions;
using Blazor.Startup.Example.Helpers.Health;
using Blazor.Startup.Example.Models.ApplicationSettings;
using Blazor.Startup.Example.Services.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blazor.Startup.Example;

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
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
                .AddEventLog();
        }
        else
        {
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug();
        }

        builder.Services.AddMemoryCache();
        builder.SetHttpClients(appSettings);

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.SetDependencyInjection(appSettings);

        builder.Services.AddAuthorization();

        builder.Services.AddHealthChecks()
            .AddCheck<StartupExampleAppHealthCheck>(HttpClientNames.StartupExample_App.ToLower());

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
        builder.Services.AddHttpClient(HttpClientNames.StartupExample_Api, c =>
        {
            c.BaseAddress = new Uri(appSettings.PageUrl);

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

        builder.Services.AddHttpClient(HttpClientNames.StartupExample_App, c =>
        {
            c.BaseAddress = new Uri(appSettings.PageUrl);

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