using FluentValidation;
using Microsoft.Data.SqlClient;
using Startup.Function.Api.Models.AppSettings;
using System.Diagnostics.CodeAnalysis;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Startup.Function.Api.Helpers.Validators;

// ReSharper disable once UnusedMember.Global
[ExcludeFromCodeCoverage]
public class AppSettingsOptionsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsOptionsValidator()
    {
        RuleFor(x => x.Logging)
            .NotNull();
        RuleFor(x => x.Logging!.LogLevel)
            .NotNull();
        RuleFor(x => x.Logging!.LogLevel!.Default)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging!.LogLevel!.Microsoft)
            .IsEnumName(typeof(LogLevel));
        RuleFor(x => x.Logging!.LogLevel!.System)
            .IsEnumName(typeof(LogLevel));

        // if the connection string is invalid 'SqlConnectionStringBuilder' will throw an exception.
        // The Contains check validates that we are pointed at the correct database.
        RuleFor(x => x.ConnectionStrings)
            .NotNull();
        RuleFor(x => x.ConnectionStrings!.DefaultConnection)
            .NotNull()
            .NotEmpty()
            .Must(s => new SqlConnectionStringBuilder(s).ConnectionString.Contains("Initial Catalog=StartupExample", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("The connection string cannot be empty, must be formatted correctly, and be pointed at the correct database.");

        RuleFor(x => x.RedactionKey)
            .NotNull()
            .NotEmpty()
            .Must(e => e != null && !e.Contains("Replace-Key"))
            .Length(48, 128);

        RuleFor(x => x.HttpClients)
            .NotNull();

        RuleFor(x => x.HttpClients!.Resilience)
            .NotNull();
        RuleFor(x => x.HttpClients!.Resilience!.BaseTimeOutInSeconds)
            .InclusiveBetween(5, 120);
    }
}