using Startup.Blazor.Server.Components.Account;
using Startup.Blazor.Server.Models.ApplicationSettings;
using Startup.Blazor.Server.Models.Identity;
using Startup.Blazor.Server.Repository.Http.Endpoints;
using Startup.Blazor.Server.Repository.Http.Endpoints.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Startup.Blazor.Server.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        service.AddDatabaseDeveloperPageExceptionFilter();

        service.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager().AddDefaultTokenProviders();

        service.AddScoped<IdentityUserAccessor>();
        service.AddScoped<IdentityRedirectManager>();
        service.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        service.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        #region Http Repositories

        service.AddTransient<IInfoPageCache, InfoPageCache>();
        service.AddScoped<IInfoPageRepository, InfoPageRepository>();

        #endregion
    }
}