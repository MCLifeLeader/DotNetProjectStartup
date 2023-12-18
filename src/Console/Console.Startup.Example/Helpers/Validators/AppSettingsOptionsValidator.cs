using Console.Startup.Example.Model.ApplicationSettings;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Console.Startup.Example.Helpers.Validators;

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

        RuleFor(x => x.ConnectionStrings.ApplicationInsights)
            .NotNull()
            .NotEmpty()
            .Must(s => s.ToLower().Contains("instrumentationkey") || s.ToLower().Contains("na"));

        // ToDo: Once key vault is live, comment back in
        //RuleFor(x => x.KeyVaultUri)
        //    .NotEmpty();
    }
}