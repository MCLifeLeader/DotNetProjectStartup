using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Startup.Common.Connection.Interfaces;
using Startup.Common.Connection;
using Startup.Function.Api.Models.AppSettings;
using Startup.Function.Api.Services;
using Startup.Function.Api.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Function.Api.DependencyRegistration;

[ExcludeFromCodeCoverage]
public static class DependencyResolution
{
    public static void RegisterDependencies(IServiceCollection services, HostBuilderContext context, AppSettings appSettings)
    {
        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            OpenApiConfigurationOptions options = new OpenApiConfigurationOptions
            {
                Info = (new ApiInfo()).GetApiVersion("1.0.0"),
                Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                OpenApiVersion = OpenApiVersionType.V3,
                IncludeRequestingHostName = true,
                ForceHttps = false,
                ForceHttp = false
            };
            return options;
        });

        services.AddTransient<IHealthService, HealthService>();
        services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
    }

    internal class ApiInfo
    {
        /// <summary>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public OpenApiInfo GetApiVersion(string version)
        {
            return new()
            {
                Title = "Startup Function Api",
                Version = $"{version}",
                Description = $"These are common functions examples. App Version: {GetType().Assembly.GetName().Version}"
            };
        }

        public Version? GetAssemblyVersion()
        {
            return GetType().Assembly.GetName().Version;
        }
    }

}