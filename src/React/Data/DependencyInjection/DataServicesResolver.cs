using Microsoft.EntityFrameworkCore;
using React.Startup.Example.Models;
using React.Startup.Example.Models.ApplicationSettings;

namespace React.Startup.Example.Data.DependencyInjection;

public static class DataServicesResolver
{
    public static void RegisterDependencies(IServiceCollection service, AppSettings appSettings)
    {
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        service.AddDatabaseDeveloperPageExceptionFilter();
    }
}