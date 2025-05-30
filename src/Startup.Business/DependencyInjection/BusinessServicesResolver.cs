using Microsoft.Extensions.DependencyInjection;

namespace Startup.Business.DependencyInjection;

public static class BusinessServicesResolver
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        //services.AddAzureClients(builder =>
        //{
        //    builder.AddBlobServiceClient(storageConnectionString).WithName(BlobStorageClientNames.STARTUP_BLOB);
        //});

        #region Repositories
        //services.AddTransient<IBlobStorageRepository, BlobStorageRepository>();
        #endregion

        #region Services
        //services.AddTransient<IBlobService, BlobService>();
        #endregion
    }
}