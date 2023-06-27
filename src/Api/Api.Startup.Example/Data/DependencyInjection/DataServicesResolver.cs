using Api.Startup.Example.Models.ApplicationSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Startup.Example.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        service.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
    }
}