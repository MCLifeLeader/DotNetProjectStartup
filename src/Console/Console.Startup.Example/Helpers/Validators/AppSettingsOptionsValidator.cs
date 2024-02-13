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

        RuleFor(x => x.KeyVaultUri)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.RedactionKey)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"))
            .Length(48, 128);

        RuleFor(x => x.FeatureManagement.HealthCheckWorker)
            .Must(_ => true);

        RuleFor(x => x.ServiceName)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.WorkerProcesses.SleepDelaySeconds)
            .InclusiveBetween(1, 120);
        RuleFor(x => x.WorkerProcesses.StartupApi.Uri)
            .NotNull()
            .NotEmpty()
            .Must(e => e.Contains("https://"));
        RuleFor(x => x.WorkerProcesses.StartupApi.Cron)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.WorkerProcesses.StartupApi.TimeOutInSeconds)
            .InclusiveBetween(5, 120);
        RuleFor(x => x.WorkerProcesses.StartupApi.Username)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.WorkerProcesses.StartupApi.Password)
            .NotNull()
            .NotEmpty()
            .Must(e => !e.Contains("Replace-Key"));

    }
}