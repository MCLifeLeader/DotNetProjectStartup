using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Startup.Web.Areas.Identity;
using Startup.Web.Models.ApplicationSettings;
using Startup.Web.Repository.Http.Endpoints;
using Startup.Web.Repository.Http.Endpoints.Interfaces;

namespace Startup.Web.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        service.AddDatabaseDeveloperPageExceptionFilter();
        service.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
        service.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

        #region Http Repositories

        service.AddTransient<IInfoPageCache, InfoPageCache>();
        service.AddScoped<IInfoPageRepository, InfoPageRepository>();

        #endregion
    }
}