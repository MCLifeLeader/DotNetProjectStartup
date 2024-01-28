using Newtonsoft.Json;

namespace Startup.Business.Models.ApplicationSettings;

public class StorageAccount
{
    public string BlobStorageUrl { get; set; }
    [JsonIgnore]
    public string BlobStorageContainer { get; set; }
}