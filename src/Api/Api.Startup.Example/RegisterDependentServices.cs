using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Api.Startup.Example.Connection.DependencyInjection;
using Api.Startup.Example.Constants;
using Api.Startup.Example.Data.DependencyInjection;
using Api.Startup.Example.Factories.DependencyInjection;
using Api.Startup.Example.Helpers.DependencyInjection;
using Api.Startup.Example.Helpers.Extensions;
using Api.Startup.Example.Helpers.Filter;
using Api.Startup.Example.Helpers.Health;
using Api.Startup.Example.Models.ApplicationSettings;
using Api.Startup.Example.Services.DependencyInjection;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Startup.Data.Repositories.DependencyInjection;

namespace Api.Startup.Example;

/// <summary>
/// This class contains static methods responsible for registering dependent services such as connection, repositories, and services in the web application's builder. class
/// </summary>
public static class RegisterDependentServices
{
    private static readonly string _swaggerName = "StartupExample API";

    /// <summary>
    /// Extension method that registers services used in the application.
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
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


        if (appSettings.CorsEnabled)
        {
            builder.Services.AddCors(options => options.AddPolicy("StartupExampleApi",
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins(appSettings.Jwt.Issuer);
                    corsPolicyBuilder.AllowAnyHeader();
                    corsPolicyBuilder.AllowAnyMethod();
                }));
        }

        // Add services to the container.
        builder.Services.AddControllersWithViews(options => { options.RespectBrowserAcceptHeader = true; })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            }).AddXmlSerializerFormatters();

        builder.Services.AddRequestDecompression();
        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

        builder.SetHttpClients(appSettings);

        if (appSettings.SwaggerEnabled)
        {
            builder.Services.AddApiVersioning(c =>
            {
                c.DefaultApiVersion = new ApiVersion(1, 0);
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.ReportApiVersions = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new ApiInfo().GetApiVersion("v1"));
                c.OperationFilter<SwaggerResponseOperationFilter>();
                c.DocumentFilter<AdditionalPropertiesDocumentFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                // Add informative documentation on API Route Endpoints for auto documentation on Swagger page.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        builder.SetDependencyInjection(appSettings);

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = appSettings.Jwt.Issuer,
                ValidAudience = appSettings.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                // Allow a +/- 5 second tolerance for the expiration date.
                ClockSkew = TimeSpan.FromSeconds(5)
            };
        });
        builder.Services.AddAuthorization();

        builder.Services.AddHealthChecks()
            .AddCheck<StartupExampleAppHealthCheck>(HttpClientNames.StartupExample_App.ToLower())
            .AddSqlServer(appSettings.ConnectionStrings.DefaultConnection);

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
        RepositoriesResolver.RegisterDependencies(builder.Services, appSettings.ConnectionStrings.DefaultConnection);

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