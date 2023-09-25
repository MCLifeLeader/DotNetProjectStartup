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
        RuleFor(x => x.Logging.LogLevel.MicrosoftAspNetCore)
            .IsEnumName(typeof(LogLevel));

        // ToDo: Once key vault is live, comment back in
        //RuleFor(x => x.KeyVaultUri)
        //    .NotEmpty();

        //RuleFor(x => x.ApplicationInsights.ConnectionString)
        //    .NotEmpty();
    }
}