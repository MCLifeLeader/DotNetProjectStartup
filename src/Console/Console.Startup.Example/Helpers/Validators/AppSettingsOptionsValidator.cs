using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console.Helpers.Validators;

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

        RuleFor(x => x.RedactionKey)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ConnectionStrings.ApplicationInsights)
            .NotNull()
            .NotEmpty()
            .Must(s => s.ToLower().Contains("instrumentationkey") || s.ToLower().Contains("na"));

        // if the connection string is invalid 'SqlConnectionStringBuilder' will throw an exception.
        // The Contains check validates that we are pointed at the EDU database.
        RuleFor(x => x.ConnectionStrings.DefaultConnection)
            .NotEmpty()
            .Must(s => new SqlConnectionStringBuilder(s).ConnectionString.Contains("Database=StartupExample", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("The connection string cannot be empty, must be formatted correctly, and be pointed at the StartupExample database.");
    }
}