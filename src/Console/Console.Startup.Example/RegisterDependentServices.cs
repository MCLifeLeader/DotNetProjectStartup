using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using Azure.Identity;
using Console.Startup.Example.BackgroundService.DependencyInjection;
using Console.Startup.Example.Connection.DependencyInjection;
using Console.Startup.Example.Factories.DependencyInjection;
using Console.Startup.Example.Helpers.Data;
using Console.Startup.Example.Helpers.DependencyInjection;
using Console.Startup.Example.Helpers.Extensions;
using Console.Startup.Example.Helpers.Filter;
using Console.Startup.Example.Model.ApplicationSettings;
using Console.Startup.Example.Repositories.DependencyInjection;
using Console.Startup.Example.Services.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Startup.Common.Constants;
using Startup.Common.Helpers.Extensions;
using Startup.Common.Models;
using Startup.Common.Models.Authorization;
using System.Net.Http.Formatting;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using System.Text;
using System.Text.Encodings.Web;

namespace Console.Startup.Example;

public static class RegisterDependentServices
{
    private static LoginToken? _token;

    public static IHostBuilder RegisterServices(this IHostBuilder builder)
    {
        AppSettings? appSettings = null;

        // Needed for windows services to find their resources to run correctly.
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        // OS Level environment variable and if environment is null fallback to Development
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                             Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                             "Development";

        builder.ConfigureAppConfiguration((hostContext, configApp) =>
            {
                #region Configuration/Key Vault

                // Note: This is not being set were I was expecting it to be.
                hostContext.HostingEnvironment.EnvironmentName = environment;

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

                if (!string.IsNullOrEmpty(configuration.GetValue<string>("KeyVaultUri")))
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
                #region Bind AppSettings for use in IOptions Pattern

                services.Configure<AppSettings>(hostContext.Configuration);
                appSettings = new AppSettings
                {
                    ConfigurationBase = hostContext.Configuration
                };
                hostContext.Configuration.Bind(appSettings);

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

                services.AddLogging();

                services.Configure<HostOptions>(options =>
                {
                    //Service Behavior in case of exceptions - defaults to StopHost
                    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                    //Host will try to wait 30 seconds before stopping the service. 
                    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
                });

                services.SetHttpClients(appSettings);
                services.SetDependencyInjection(appSettings);
                services.AddFeatureManagement();

                services.AddRedaction(x =>
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
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.ClearProviders();

                logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));

                if (appSettings != null && !appSettings.ConnectionStrings.ApplicationInsights.ToLower().Contains("na"))
                {
                    logging.AddApplicationInsights(
                        configureTelemetryConfiguration: (config) =>
                            config.ConnectionString = appSettings.ConnectionStrings.ApplicationInsights,
                        configureApplicationInsightsLoggerOptions: (options) => { });
                }

                // EventLog is only available in a Windows environment
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //ToDo: Consider removing windows Event Logging in favor of just logging to App Insights
                    logging
                        .AddEventLog()
                        .EnableRedaction();
                }

#if DEBUG || DEVELOPMENT
                logging
                    .AddConsole()
                    .AddJsonConsole(o => o.JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = true,
                        Encoder = JavaScriptEncoder.Default
                    })
                    .AddDebug();
#endif
            })
            .UseWindowsService(o => { o.ServiceName = appSettings!.ServiceName; });

#if DEBUG || DEVELOPMENT
        // If registered as a windows service. This will cause the service to fail to start.
        // if the console window is closed help kill running processes.
        builder.UseConsoleLifetime();
#endif
        return builder;
    }

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

    private static void SetHttpClients(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_API, c =>
        {
            c.BaseAddress = new Uri(appSettings.WorkerProcesses.StartupApi.Uri);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
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

        services.AddHttpClient(HttpClientNames.STARTUPEXAMPLE_HOME, c =>
        {
            c.BaseAddress = new Uri(appSettings.WorkerProcesses.RemoteServerConnection.Uri);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            c.Timeout = TimeSpan.FromSeconds(appSettings.WorkerProcesses.StartupApi.TimeOutInSeconds);
        }).ConfigurePrimaryHttpMessageHandler(c =>
        {
            HttpClientHandler h = new HttpClientHandler
            {
                UseProxy = false
            };
            return h;
        });
    }

    private static LoginToken? GetAuthToken(IServiceCollection services, HttpClient httpClient, UserLoginModel userLoginModel)
    {
        LoginToken? token = null;

        ILogger<Program> logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

        try
        {
            HttpResponseMessage response = httpClient.PostAsync(
                "v1.0/Auth/Login",
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