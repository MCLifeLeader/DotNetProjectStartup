using Startup.Business.Models;

namespace Startup.Business.Services.Interfaces;

public interface IBlobService
{
    Task<MediaFileMetaData> UploadFileToAzureStorageAsync(Guid userId, Guid agencyId, string fileToUpload);
}