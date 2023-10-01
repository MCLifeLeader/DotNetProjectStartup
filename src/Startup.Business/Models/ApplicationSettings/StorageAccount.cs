using Newtonsoft.Json;
using Startup.Common.Helpers.Extensions;

namespace Startup.Business.Models.ApplicationSettings;

public class StorageAccount
{
    public string BlobStorageUrl { get; set; }
    [JsonIgnore]
    public string BlobStorageConnection { get; set; }
    public string BlobStorageConnectionMask => BlobStorageConnection.Mask();
    public string BlobStorageContainer { get; set; }
}