using Asp.Versioning;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Startup.Api.Connection.DependencyInjection;
using Startup.Api.Data.DependencyInjection;
using Startup.Api.Factories.DependencyInjection;
using Startup.Api.Helpers.DependencyInjection;
using Startup.Api.Helpers.Extensions;
using Startup.Api.Helpers.Filter;
using Startup.Api.Helpers.Health;
using Startup.Api.Models.ApplicationSettings;
using Startup.Api.Services.DependencyInjection;
using Startup.Business.DependencyInjection;
using Startup.Common.Constants;
using Startup.Common.Helpers.Data;
using Startup.Common.Helpers.Filter;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Startup.Api;

/// <summary>
/// This class contains static methods responsible for registering dependent services such as connection, repositories, and services in the web application's builder. class
/// </summary>
public static class RegisterDependentServices
{
    private static readonly string _swaggerName = "StartupExample API";

    private static AppSettings? _appSettings;

    /// <summary>
    /// Extension method that registers services used in the application.
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <param name="appSettings"></param>
    /// <returns>The configured web application builder.</returns>
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

        if (!_appSettings.ConnectionStrings.ApplicationInsights.ToLower().Contains("na"))
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
        });

        #endregion

        if (_appSettings.FeatureManagement.CorsEnabled)
        {
            builder.Services.AddCors(options => options.AddPolicy("StartupExampleApi",
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins(_appSettings.Jwt.Issuer);
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

        if (_appSettings.FeatureManagement.SwaggerEnabled)
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
                c.SwaggerDoc("v2", new ApiInfo().GetApiVersion("v2"));
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

        builder.Services.AddFeatureManagement();

        builder.SetDependencyInjection(_appSettings);

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
                ValidIssuer = _appSettings.Jwt.Issuer,
                ValidAudience = _appSettings.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key)),
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
            .AddCheck<OpenAiHealthCheck>(HttpClientNames.OPEN_AI_API_HEALTH.ToLower())
            .AddSqlServer(_appSettings.ConnectionStrings.DefaultConnection);

        appSettings = _appSettings;
        return builder;
    }

    private static void SetDependencyInjection(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        // connection
        ConnectionResolver.RegisterDependencies(builder.Services, appSettings);

        // services
        ServicesResolver.RegisterDependencies(builder.Services, appSettings);
        BusinessServicesResolver.RegisterDependencies(builder.Services);

        // helpers
        HelpersResolver.RegisterDependencies(builder.Services, appSettings);

        // repositories
        DataServicesResolver.RegisterDependencies(builder.Services, appSettings);

        //factories
        FactoriesResolver.RegisterDependencies(builder.Services, appSettings);
    }

    private static void SetHttpClients(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddHttpClient(HttpClientNames.OPEN_AI_API_HEALTH, c =>
        {
            c.BaseAddress = new Uri(appSettings.HealthCheckEndpoints.OpenAi);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.Timeout = TimeSpan.FromSeconds(appSettings.HealthCheckEndpoints.TimeoutInSeconds);
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
                Description = $"API Swagger Documentation, &copy; 2022 - {DateTime.UtcNow:yyyy} - {_swaggerName} " +
                              $"- <a class=\"fw-bold\" href=\"https://github.com/MCLifeLeader/ePortfolio\" target=\"_blank\" rel=\"noopener\">ePortfolio</a> " +
                              $"- Build Version: {GetType().Assembly.GetName().Version}"
            };
        }
    }
}