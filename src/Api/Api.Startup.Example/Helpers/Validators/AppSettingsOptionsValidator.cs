using Api.Startup.Example.Models.ApplicationSettings;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace Api.Startup.Example.Helpers.Validators;

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

        RuleFor(x => x.ConnectionStrings.ApplicationInsights)
            .NotNull()
            .NotEmpty()
            .Must(s => s.ToLower().Contains("instrumentationkey") || s.ToLower().Contains("na"));

        // if the connection string is invalid 'SqlConnectionStringBuilder' will throw an exception.
        // The Contains check validates that we are pointed at the EDU database.
        RuleFor(x => x.ConnectionStrings.DefaultConnection)
            .NotEmpty()
            .Must(s => new SqlConnectionStringBuilder(s).ConnectionString.Contains("Initial Catalog=StartupExample", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("The connection string cannot be empty, must be formatted correctly, and be pointed at the StartupExample database.");

        RuleFor(x => x.AllowedHosts)
            .NotEmpty();

        // ToDo: Once key vault is live, comment back in
        //RuleFor(x => x.KeyVaultUri)
        //    .NotEmpty();

        RuleFor(x => x.SwaggerEnabled)
            .Must(_ => true);
        RuleFor(x => x.DisplayConfiguration)
            .Must(_ => true);

        RuleFor(x => x.CacheDurationInSeconds)
            .InclusiveBetween(5, 120);

        RuleFor(x => x.Jwt.Key)
            .NotNull()
            .NotEmpty()
            .Length(64, 1024);
        RuleFor(x => x.Jwt.Issuer)
            .NotNull()
            .NotEmpty()
            .Must(e=>e.Contains("https://"));
        RuleFor(x => x.Jwt.Audience)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.Jwt.ExpireInMinutes)
            .InclusiveBetween(1, 600);
        RuleFor(x => x.Jwt.Subject)
            .NotNull()
            .NotEmpty();
    }
}