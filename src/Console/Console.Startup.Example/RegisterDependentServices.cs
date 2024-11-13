using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Startup.Common.Constants;
using Startup.Common.Helpers.Data;
using Startup.Common.Helpers.Extensions;
using Startup.Common.Helpers.Filter;
using Startup.Common.Models;
using Startup.Common.Models.Authorization;
using Startup.Console.BackgroundService.DependencyInjection;
using Startup.Console.Connection.DependencyInjection;
using Startup.Console.Factories.DependencyInjection;
using Startup.Console.Helpers.DependencyInjection;
using Startup.Console.Helpers.Extensions;
using Startup.Console.Model.ApplicationSettings;
using Startup.Console.Repositories.DependencyInjection;
using Startup.Console.Services.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Startup.Console;

/// <summary>
/// Registers the dependent services for the console application.
/// </summary>
public static class RegisterDependentServices
{
    /// <summary>
    /// The login token.
    /// </summary>
    private static LoginToken? _token;

    /// <summary>
    /// The application settings.
    /// </summary>
    private static AppSettings? _appSettings = null;

    /// <summary>
    /// Registers the services for the console application.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    /// <param name="appSettings">The application settings.</param>
    /// <returns>The host builder.</returns>
    public static IHostBuilder RegisterServices(this IHostBuilder builder, out AppSettings? appSettings)
    {
        // Needed for windows services to find their resources to run correctly.
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        #region Environment Variable for DOTNET

        // OS Level environment variable and if environment is null fallback to Development
        string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                              Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // ToDo: launchSettings.json does not seem to take effect for console applications
        if (string.IsNullOrEmpty(environment))
        {
#if DEBUG || DEVELOPMENT
            builder.UseEnvironment(Environments.Development);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", Environments.Development);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Development);
#elif STAGING
            builder.UseEnvironment(Environments.Staging);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", Environments.Staging);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Staging);
#elif PRODUCTION || RELEASE
            builder.UseEnvironment(Environments.Production);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", Environments.Production);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Production);
#endif
        }

        #endregion

        builder.ConfigureAppConfiguration((hostContext, configApp) =>
            {
                #region Configuration/Key Vault

                // Note: This is not being set where I was expecting it to be.
                hostContext.HostingEnvironment.EnvironmentName = environment ?? "Production";

                configApp.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true);

                // Pre-load developer secrets if present
                if (!hostContext.HostingEnvironment.IsProduction())
                {
                    configApp.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
                }

                // Build the initial configuration settings to be used for the Key Vault registration
                IConfiguration configuration = configApp.Build();

                string keyVaultUri = configuration.GetValue<string>("KeyVaultUri")!;
                if (!string.IsNullOrEmpty(keyVaultUri) && !keyVaultUri.ToLower().Contains("na"))
                {
                    configApp.AddAzureKeyVault(new Uri(configuration.GetValue<string>("KeyVaultUri")!),
                        new DefaultAzureCredential());
                }

                // Import Environment Variables from the Host Server / Service
                configApp.AddEnvironmentVariables();

                if (hostContext.HostingEnvironment.IsDevelopment() ||
                    hostContext.HostingEnvironment.IsEnvironment("Debug"))
                {
                    configApp.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
                }

                #endregion
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();

                #region Bind AppSettings for use in IOptions Pattern

                services.Configure<AppSettings>(hostContext.Configuration);
                _appSettings = new AppSettings
                {
                    ConfigurationBase = hostContext.Configuration
                };
                hostContext.Configuration.Bind(_appSettings);

                // Adds the Fluent Validation to DI.
                services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

                // Validate the app settings model
                services
                    .AddOptions<AppSettings>()
                    .Bind(hostContext.Configuration)
                    .ValidateDataAnnotations()
                    .ValidateFluently()
                    .ValidateOnStart();

                services.AddSingleton(hostContext.Configuration);

                #endregion

                services.Configure<HostOptions>(options =>
                {
                    //Service Behavior in case of exceptions - defaults to StopHost
                    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                    //Host will try to wait 30 seconds before stopping the service. 
                    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
                });

                services.ConfigureHttpClientDefaults(http =>
                {
                    // Turn on resilience by default
                    http.AddStandardResilienceHandler();
                });

                services.SetHttpClients(_appSettings);
                services.SetDependencyInjection(_appSettings);
                services.AddFeatureManagement();

                #region Logging Setup

                services.AddRedaction(x =>
                {
                    x.SetRedactor<ErasingRedactor>(new DataClassificationSet(DataTaxonomy.SensitiveData));

                    x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PartialSensitiveData));

                    x.SetHmacRedactor(o =>
                    {
                        o.Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.RedactionKey));
                        o.KeyId = 1776;
                    }, new DataClassificationSet(DataTaxonomy.Pii));

                    x.SetFallbackRedactor<NullRedactor>();
                });

                if (_appSettings != null && !_appSettings.ConnectionStrings.ApplicationInsights.ToLower().Contains("na"))
                {
                    services.AddApplicationInsightsTelemetry();
                }

                #endregion
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                #region Logging Setup

                logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));

                if (_appSettings != null && !_appSettings.ConnectionStrings.ApplicationInsights.ToLower().Contains("na"))
                {
                    logging.AddApplicationInsights(
                        configureTelemetryConfiguration: (config) =>
                            config.ConnectionString = _appSettings.ConnectionStrings.ApplicationInsights,
                        configureApplicationInsightsLoggerOptions: (options) => { });
                }

                // EventLog is only available in a Windows environment
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //ToDo: Consider removing windows Event Logging in favor of just logging to App Insights
                    logging.AddEventLog();
                }

                if (hostContext.HostingEnvironment.EnvironmentName == Environments.Development)
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

                if (_appSettings is { FeatureManagement.OpenTelemetryEnabled: true })
                {
                    logging.ClearProviders();
                    logging.AddOpenTelemetry(x =>
                    {
                        x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                            .AddService(Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown")
                            .AddAttributes(new Dictionary<string, object>()
                            {
                                ["deployment.environment"] = hostContext.HostingEnvironment.EnvironmentName,
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

                logging.EnableRedaction();

                #endregion
            })
            .UseWindowsService(o => { o.ServiceName = _appSettings!.ServiceName; });

        if (environment == Environments.Development)
        {
            // If registered as a windows service. This will cause the service to fail to start.
            // if the console window is closed help kill running processes.
            builder.UseConsoleLifetime();
        }

        appSettings = _appSettings;
        return builder;
    }

    /// <summary>
    /// Sets the dependency injection for the services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="appSettings">The application settings.</param>
    private static void SetDependencyInjection(this IServiceCollection services, AppSettings appSettings)
    {
        //// http client wrapper etc
        ConnectionResolver.RegisterDependencies(services, appSettings);

        //// services
        BackgroundServicesResolver.RegisterDependencies(services);
        ServicesResolver.RegisterDependencies(services);

        //// helpers
        HelpersResolver.RegisterDependencies(services, appSettings);

        //// repositories
        RepositoriesResolver.RegisterDependencies(services);

        //// factories
        FactoriesResolver.RegisterDependencies(services);
    }

    /// <summary>
    /// Sets the HTTP clients for the services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="appSettings">The application settings.</param>
    private static void SetHttpClients(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_API, c =>
        {
            c.BaseAddress = new Uri(appSettings.WorkerProcesses.StartupApi.Uri);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.Timeout = TimeSpan.FromSeconds(appSettings.WorkerProcesses.StartupApi.TimeOutInSeconds);

            UserLoginModel userLogin = new UserLoginModel()
            {
                Username = appSettings.WorkerProcesses.StartupApi.Username,
                Password = appSettings.WorkerProcesses.StartupApi.Password,
                DisplayName = appSettings.WorkerProcesses.StartupApi.Username,
            };

            if (!string.IsNullOrEmpty(_token?.Token))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(_token?.Token);

                if (token.ValidTo < DateTime.UtcNow)
                {
                    _token = GetAuthToken(services, c, userLogin);
                }
            }
            else
            {
                _token = GetAuthToken(services, c, userLogin);
            }

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token?.Token);

        }).ConfigurePrimaryHttpMessageHandler(c =>
        {
            HttpClientHandler h = new HttpClientHandler
            {
                UseProxy = false
            };
            return h;
        });
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="userLoginModel">The user login model.</param>
    /// <returns>The login token.</returns>
    private static LoginToken? GetAuthToken(IServiceCollection services, HttpClient httpClient, UserLoginModel userLoginModel)
    {
        LoginToken? token = null;

        ILogger<Program> logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

        try
        {
            HttpResponseMessage response = httpClient.PostAsync(
                "Auth/Login",
                userLoginModel,
                new JsonMediaTypeFormatter()).Result;

            if (response.IsSuccessStatusCode)
            {
                string rawToken = response.Content.ReadAsStringAsync().Result;
                token = rawToken.FromJson<LoginToken>();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting auth token");
        }

        return token;
    }
}
