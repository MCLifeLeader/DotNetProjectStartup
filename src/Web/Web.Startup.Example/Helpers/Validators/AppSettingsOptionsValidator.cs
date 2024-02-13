using FluentValidation;
using Microsoft.Data.SqlClient;
using Startup.Web.Models.ApplicationSettings;

namespace Startup.Web.Helpers.Validators;

public class AppSettingsOptionsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsOptionsValidator()
    {
        RuleFor(x => x.Logging.LogLevel.Default)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging.LogLevel.Microsoft)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging.LogLevel.MicrosoftAspNetCore)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging.LogLevel.System)
            .IsEnumName(typeof(LogLevel));

        RuleFor(x => x.FeatureManagement.InformationEndpoints)
            .Must(_ => true);
        RuleFor(x => x.FeatureManagement.SqlDebugger)
            .Must(_ => true);
        RuleFor(x => x.FeatureManagement.DisplayConfiguration)
            .Must(_ => true);

        RuleFor(x => x.KeyVaultUri)
            .NotEmpty();

        RuleFor(x => x.RedactionKey)
            .NotNull()
            .NotEmpty();

        // if the connection string is invalid 'SqlConnectionStringBuilder' will throw an exception.
        // The Contains check validates that we are pointed at the correct database.
        RuleFor(x => x.ConnectionStrings.DefaultConnection)
            .NotNull()
            .NotEmpty()
            .Must(s => new SqlConnectionStringBuilder(s).ConnectionString.Contains("Initial Catalog=StartupExample", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("The connection string cannot be empty, must be formatted correctly, and be pointed at the correct database.");
        RuleFor(x => x.ConnectionStrings.ApplicationInsights)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"));

        RuleFor(x => x.HttpClients.OpenAi.BaseUrl)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.HttpClients.OpenAi.ApiKey)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"));
        RuleFor(x => x.HttpClients.OpenAi.AiModel)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.HttpClients.OpenAi.TimeoutInSeconds)
            .InclusiveBetween(5, 120);
        RuleFor(x => x.HttpClients.OpenAi.CacheDurationInSeconds)
            .InclusiveBetween(5, 120);

        RuleFor(x => x.HttpClients.AzureOpenAi.BaseUrl)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.HttpClients.AzureOpenAi.ApiKey)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"));
        RuleFor(x => x.HttpClients.AzureOpenAi.AiModel)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.HttpClients.AzureOpenAi.TimeoutInSeconds)
            .InclusiveBetween(5, 120);
        RuleFor(x => x.HttpClients.AzureOpenAi.CacheDurationInSeconds)
            .InclusiveBetween(5, 120);

        RuleFor(x => x.HealthCheckEndpoints.OpenAi)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.HealthCheckEndpoints.TimeoutInSeconds)
            .InclusiveBetween(5, 120);

        RuleFor(x => x.AllowedHosts)
            .NotEmpty();
    }
}