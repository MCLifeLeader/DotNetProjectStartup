using FluentValidation;
using Microsoft.Data.SqlClient;
using Startup.Api.Models.ApplicationSettings;

namespace Startup.Api.Helpers.Validators;

public class AppSettingsOptionsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsOptionsValidator()
    {
        RuleFor(x => x.Logging.LogLevel.Default)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging.LogLevel.Microsoft)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging.LogLevel.System)
            .IsEnumName(typeof(LogLevel));

        // if the connection string is invalid 'SqlConnectionStringBuilder' will throw an exception.
        // The Contains check validates that we are pointed at the correct database.
        RuleFor(x => x.ConnectionStrings.DefaultConnection)
            .NotNull()
            .NotEmpty()
            .Must(s => new SqlConnectionStringBuilder(s).ConnectionString.Contains("Initial Catalog=AiCoaches", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("The connection string cannot be empty, must be formatted correctly, and be pointed at the correct database.");
        RuleFor(x => x.ConnectionStrings.ApplicationInsights)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"));

        RuleFor(x => x.KeyVaultUri)
            .NotEmpty();

        RuleFor(x => x.FeatureManagement.SwaggerEnabled)
            .Must(_ => true);
        RuleFor(x => x.FeatureManagement.DisplayConfiguration)
            .Must(_ => true);
        RuleFor(x => x.FeatureManagement.CorsEnabled)
            .Must(_ => true);

        RuleFor(x => x.CacheDurationInSeconds)
    .InclusiveBetween(5, 120);

        RuleFor(x => x.RedactionKey)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"))
            .Length(48, 128);

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

        RuleFor(x => x.Jwt.Key)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"))
            .Length(64, 1024);
        RuleFor(x => x.Jwt.Issuer)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.Jwt.Audience)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.Jwt.ExpireInMinutes)
            .InclusiveBetween(5, 600);
        RuleFor(x => x.Jwt.Subject)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.AllowedHosts)
            .NotEmpty();
    }
}