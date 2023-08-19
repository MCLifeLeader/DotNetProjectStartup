using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using Azure.Identity;
using Console.Startup.Example.Connection.DependencyInjection;
using Console.Startup.Example.Constants;
using Console.Startup.Example.Factories.DependencyInjection;
using Console.Startup.Example.Helpers.DependencyInjection;
using Console.Startup.Example.Helpers.Extensions;
using Console.Startup.Example.Model.ApplicationSettings;
using Console.Startup.Example.Repositories.DependencyInjection;
using Console.Startup.Example.Service.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Console.Startup.Example;

public static class RegisterDependentServices
{
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
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.ClearProviders();

                // EventLog is only available in a Windows environment
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Log to the Windows Event Log if on a Windows OS
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                        .AddConsole()
                        .AddDebug()
                        .AddEventLog();
                }
                else
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                        .AddConsole()
                        .AddDebug();
                }
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
        ConnectionResolver.RegisterDependencies(services);

        //// services
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
        services.AddHttpClient(HttpClientNames.RemoteHostServerClient, c =>
        {
            c.BaseAddress = new Uri(appSettings.DataConnection.Uri);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            c.Timeout = TimeSpan.FromSeconds(appSettings.DataConnection.TimeOutInSeconds);
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