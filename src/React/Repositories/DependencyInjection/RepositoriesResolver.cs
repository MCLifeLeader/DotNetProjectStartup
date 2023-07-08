using Microsoft.EntityFrameworkCore;
using React.Startup.Example.Models.ApplicationSettings;
using React.Startup.Example.Repositories.Db;
using React.Startup.Example.Repositories.Db.Interfaces;
using React.Startup.Example.Repositories.Db.Persistence;

namespace React.Startup.Example.Repositories.DependencyInjection;

public static class RepositoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        #region Database Repositories

        service.AddDbContext<StartupExampleContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

        service.AddScoped<IStartupExampleContext, StartupExampleContext>();

        service.AddScoped<IAgencyRepository, AgencyRepository>();
        service.AddScoped<IAuthenticationLogRepository, AuthenticationLogRepository>();
        service.AddScoped<IAuthenticationStatusRepository, AuthenticationStatusRepository>();
        service.AddScoped<IUserToAgencyRepository, UserToAgencyRepository>();

        #endregion
    }
}