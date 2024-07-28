using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Startup.Api.Models.ApplicationSettings;
using Startup.Common.Constants;
using Startup.Common.Helpers.Extensions;
using Startup.Data.Repositories.DependencyInjection;

namespace Startup.Api.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        // ASP.NET Identity
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);

            if (service.BuildServiceProvider().GetService<IFeatureManager>()!.IsEnabledAsync(FeatureFlags.SQL_DEBUGGER).Result)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();


                options.UseLoggerFactory(LoggerSupport.GetLoggerFactory(service));
            }
        });

        service.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


        #region Database Repositories

        RepositoriesResolver.RegisterDependencies(service, appSettings.ConnectionStrings.DefaultConnection);

        #endregion
    }
}