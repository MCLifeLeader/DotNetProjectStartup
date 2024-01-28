using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Startup.Business.Models.ApplicationSettings;
using Startup.Business.Repository;
using Startup.Business.Repository.Interfaces;
using Startup.Business.Services;
using Startup.Business.Services.Interfaces;
using Startup.Common.Constants;

namespace Startup.Business.DependencyInjection;

public static class BusinessServicesResolver
{
    public static void RegisterDependencies(IServiceCollection services, string storageConnectionString)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(storageConnectionString).WithName(BlobStorageClientNames.STARTUP_BLOB);
        });

        #region Repositories
        services.AddTransient<IBlobStorageRepository, BlobStorageRepository>();
        #endregion

        #region Services
        services.AddTransient<IBlobService, BlobService>();
        #endregion
    }
}