using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using React.Startup.Example.Data;
using React.Startup.Example.Models;

namespace React.Startup.Example;

public static class RegisterDependentServices
{
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        builder.Services.AddAuthentication()
            .AddIdentityServerJwt();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        return builder;
    }
}