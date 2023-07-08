using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using React.Startup.Example.Connection.DependencyInjection;
using React.Startup.Example.Constants;
using React.Startup.Example.Data;
using React.Startup.Example.Data.DependencyInjection;
using React.Startup.Example.Factories.DependencyInjection;
using React.Startup.Example.Helpers.DependencyInjection;
using React.Startup.Example.Helpers.Extensions;
using React.Startup.Example.Helpers.Filter;
using React.Startup.Example.Helpers.Health;
using React.Startup.Example.Models;
using React.Startup.Example.Models.ApplicationSettings;
using React.Startup.Example.Repositories.DependencyInjection;
using React.Startup.Example.Services.DependencyInjection;

namespace React.Startup.Example;

public static class RegisterDependentServices
{
    private static readonly string _swaggerName = "StartupExample React";

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
        //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //{
        //    builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
        //        .AddConsole()
        //        .AddDebug()
        //        .AddEventLog();
        //}
        //else
        //{
        //    builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
        //        .AddConsole()
        //        .AddDebug();
        //}

        builder.SetDependencyInjection(appSettings);

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
        builder.Services.AddAuthentication().AddIdentityServerJwt();

        // Add services to the container.
        builder.Services.AddControllersWithViews(/*options => { options.RespectBrowserAcceptHeader = true; }*/)
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    options.SerializerSettings.Formatting = Formatting.Indented;
            //    options.SerializerSettings.Converters.Add(new StringEnumConverter());
            //})
            ;

        builder.Services.AddRazorPages();

        builder.SetHttpClients(appSettings);

        //if (appSettings.SwaggerEnabled)
        //{
        //    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //    builder.Services.AddEndpointsApiExplorer();
        //    builder.Services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new ApiInfo().GetApiVersion("v1"));
        //        c.OperationFilter<SwaggerResponseOperationFilter>();
        //        c.DocumentFilter<AdditionalPropertiesDocumentFilter>();

        //        // Add informative documentation on API Route Endpoints for auto documentation on Swagger page.
        //        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //        c.IncludeXmlComments(xmlPath);
        //    });
        //}

        //builder.Services.AddRequestDecompression();
        //builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

        //builder.Services.AddHealthChecks()
        //    .AddCheck<StartupExampleAppHealthCheck>(HttpClientNames.StartupExample_App.ToLower())
        //    .AddSqlServer(appSettings.ConnectionStrings.DefaultConnection);

        return builder;
    }

    private static void SetDependencyInjection(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        // connection
        ConnectionResolver.RegisterDependencies(builder.Services, appSettings);

        // services
        ServicesResolver.RegisterDependencies(builder.Services, appSettings);

        // helpers
        HelpersResolver.RegisterDependencies(builder.Services, appSettings);

        // identity
        DataServicesResolver.RegisterDependencies(builder.Services, appSettings);

        // repositories
        RepositoriesResolver.RegisterDependencies(builder.Services, appSettings);

        //factories
        FactoriesResolver.RegisterDependencies(builder.Services, appSettings);
    }

    private static void SetHttpClients(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddHttpClient(HttpClientNames.StartupExample_Home, c =>
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

    internal class ApiInfo
    {
        /// <summary>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public OpenApiInfo GetApiVersion(string version)
        {
            return new OpenApiInfo
            {
                Title = $"{_swaggerName} {version}",
                Version = $"{version}",
                Description = $"API Swagger Documentation, &copy; 2022 - {DateTime.UtcNow:yyyy} - {_swaggerName} - " +
                              $"Build Version: {GetType().Assembly.GetName().Version}"
            };
        }
    }
}