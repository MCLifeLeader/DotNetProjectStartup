using Blazor.Startup.Example.Components.Account;
using Blazor.Startup.Example.Models.ApplicationSettings;
using Blazor.Startup.Example.Models.Identity;
using Blazor.Startup.Example.Repository.Http.Endpoints;
using Blazor.Startup.Example.Repository.Http.Endpoints.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Startup.Example.Data.DependencyInjection;

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

        //service.AddTransient<ICanaryPageCache, CanaryPageCache>();
        //service.AddScoped<ICanaryPageRepository, CanaryPageRepository>();

        #endregion
    }
}