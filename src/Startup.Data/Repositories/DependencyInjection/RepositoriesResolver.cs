using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Startup.Data.Repositories.Db;
using Startup.Data.Repositories.Db.Interfaces;
using Startup.Data.Repositories.Db.Persistence;

namespace Startup.Data.Repositories.DependencyInjection;

public static class RepositoriesResolver
{
    public static void RegisterDependencies(IServiceCollection service, string connectionString)
    {
        #region Database Repositories

        service.AddDbContext<StartupExampleContext>(options => options.UseSqlServer(connectionString));

        service.AddScoped<IStartupExampleContext, StartupExampleContext>();

        service.AddScoped<IAgencyRepository, AgencyRepository>();
        service.AddScoped<IAuthenticationLogRepository, AuthenticationLogRepository>();
        service.AddScoped<IAuthenticationStatusRepository, AuthenticationStatusRepository>();
        service.AddScoped<IUserToAgencyRepository, UserToAgencyRepository>();

        #endregion
    }
}