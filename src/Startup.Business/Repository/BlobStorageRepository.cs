using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Startup.Business.Models;
using Startup.Business.Models.ApplicationSettings;
using Startup.Business.Repository.Interfaces;
using Startup.Common.Constants;

namespace Startup.Business.Repository;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly ILogger<BlobStorageRepository> _logger;
    private readonly BlobContainerClient _containerClient;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly BlobServiceClient _blobServiceClient;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly StorageAccount _appSettings;

    public BlobStorageRepository(ILogger<BlobStorageRepository> logger,
        StorageAccount appSettings,
        IAzureClientFactory<BlobServiceClient> clientFactory)
    {
        _logger = logger;
        _appSettings = appSettings;
        _blobServiceClient = clientFactory.CreateClient(BlobStorageClientNames.STARTUP_BLOB);
        _containerClient = _blobServiceClient.GetBlobContainerClient(_appSettings.BlobStorageContainer);
    }

    public async Task<MediaFileMetaData> UploadFileToAzureBlobAsync(string sourceFileAndPath, MediaContentObject contentDetails, bool deleteFile = false)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(UploadFileToAzureBlobAsync));

        string folderPath = GetFolderPath(
            contentDetails.AgencyId,
            contentDetails.MediaId,
            contentDetails.FileExtension,
            contentDetails.FileDate.Year,
            contentDetails.FileDate.Month);

        MediaFileMetaData metaData = new MediaFileMetaData();

        BlobClient blobClient = _containerClient.GetBlobClient(folderPath);

        await using FileStream uploadFileStream = new FileStream(sourceFileAndPath, FileMode.Open);
        await blobClient.UploadAsync(uploadFileStream, true);
        uploadFileStream.Close();

        metaData.Uri = blobClient.Uri.AbsoluteUri;
        metaData.FileAndPath = blobClient.Name;
        metaData.FileName = blobClient.Name.Substring(blobClient.Name.LastIndexOf('/') + 1);
        // ToDo: Update with actual content type.
        metaData.ContentType = "application/octet-stream";

        // delete local file if deleteFile flag is set to true.
        if (deleteFile)
        {
            File.Delete(sourceFileAndPath);
        }

        return metaData;
    }

    public async Task DownloadFileFromAzureBlobAsync(string fileNameAndPath, string fileName)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(DownloadFileFromAzureBlobAsync));

        BlobClient blobClient = _containerClient.GetBlobClient(fileNameAndPath);

        await using FileStream downloadFileStream = new FileStream(fileName, FileMode.Create);
        await blobClient.DownloadToAsync(downloadFileStream);
        downloadFileStream.Close();
    }

    public async Task DeleteFileFromAzureBlobAsync(string fileNameAndPath)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(DeleteFileFromAzureBlobAsync));

        BlobClient blobClient = _containerClient.GetBlobClient(fileNameAndPath);

        await blobClient.DeleteAsync();
    }

    public async Task UpdateFileAccessibilityInAzureBlobAsync(string fileNameAndPath, bool isPublic)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(UpdateFileAccessibilityInAzureBlobAsync));

        BlobClient blobClient = _containerClient.GetBlobClient(fileNameAndPath);

        // Set the blob's access level to either public or private based on the isPublic flag.
        await blobClient.SetAccessTierAsync(isPublic ? AccessTier.Hot : AccessTier.Cool);
    }

    /// <summary>
    /// Get the folder path in which the file will be uploaded in Blob Storage.
    /// Using the first two then 2nd two characters from the agency Guid to make it slightly easier to find the file.
    /// </summary>
    /// <param name="agencyId"></param>
    /// <param name="mediaId"></param>
    /// <param name="extension"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    private string GetFolderPath(Guid agencyId, Guid mediaId, string extension, int year, int month)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GetFolderPath));

        // Create a folder path based on the first two characters of the agencyId then the second two.
        string firstTwo = agencyId.ToString().Substring(0, 2);
        string secondTwo = agencyId.ToString().Substring(2, 2);

        // Construct the folder path in which file will be uploaded in Blob Storage.
        // The year and month are based on the date the file was created.
        return $"/{firstTwo}/{secondTwo}/{agencyId}/{year}/{month}/{mediaId}.{extension}";
    }
}