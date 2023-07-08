using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Startup.Example.Areas.Identity;
using Web.Startup.Example.Models.ApplicationSettings;
using Web.Startup.Example.Repository.Http.Endpoints;
using Web.Startup.Example.Repository.Http.Endpoints.Interfaces;

namespace Web.Startup.Example.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        service.AddDatabaseDeveloperPageExceptionFilter();
        service.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
        service.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

        #region Http Repositories

        service.AddTransient<ICanaryPageCache, CanaryPageCache>();
        service.AddScoped<ICanaryPageRepository, CanaryPageRepository>();

        #endregion
    }
}