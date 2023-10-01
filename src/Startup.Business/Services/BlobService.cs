using Microsoft.Extensions.Logging;
using Startup.Business.Models;
using Startup.Business.Repository.Interfaces;
using Startup.Business.Services.Interfaces;
using Startup.Data.Models.Db.dboSchema;

namespace Startup.Business.Services;

public class BlobService : IBlobService
{
    private readonly ILogger<BlobService> _logger;
    private readonly IBlobStorageRepository _blobStorageRepository;

    public BlobService(ILogger<BlobService> logger,
        IBlobStorageRepository blobStorageRepository)
    {
        _logger = logger;
        _blobStorageRepository = blobStorageRepository;
    }

    public async Task<MediaFileMetaData> UploadFileToAzureStorageAsync(Guid userId, Guid agencyId, string fileToUpload)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(UploadFileToAzureStorageAsync));

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Empty Guid not allowed", nameof(userId));
        }

        if (agencyId == Guid.Empty)
        {
            throw new ArgumentException("Empty Guid not allowed", nameof(agencyId));
        }

        //ToDo: Update data in database with actual values.
        MediaContent content = new MediaContent();

        var mediaContent = new MediaContentObject()
        {
            AgencyId = agencyId,
            MediaId = Guid.Empty,
            FileExtension = "wmv",
            FileDate = DateTime.Now
        };

        MediaFileMetaData results = await _blobStorageRepository.UploadFileToAzureBlobAsync(fileToUpload, mediaContent);

        return results;
    }
}