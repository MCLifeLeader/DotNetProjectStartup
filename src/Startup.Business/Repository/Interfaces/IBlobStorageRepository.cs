using Startup.Business.Models;

namespace Startup.Business.Repository.Interfaces;

public interface IBlobStorageRepository
{
    Task<MediaFileMetaData> UploadFileToAzureBlobAsync(string sourceFileAndPath, MediaContentObject contentDetails, bool deleteFile = false);
    Task DownloadFileFromAzureBlobAsync(string fileNameAndPath, string fileName);
    Task DeleteFileFromAzureBlobAsync(string fileNameAndPath);
    Task UpdateFileAccessibilityInAzureBlobAsync(string fileNameAndPath, bool isPublic);
}