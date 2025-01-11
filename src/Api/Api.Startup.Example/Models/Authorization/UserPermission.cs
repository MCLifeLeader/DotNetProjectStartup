using Newtonsoft.Json;

namespace Startup.Api.Models.Authorization;

public class UserPermission
{
    [JsonProperty]
    public string? AccountId { get; set; }

    [JsonProperty(PropertyName = "PermissionName")]
    public string? PermissionNameString { get; set; }

    [JsonIgnore]
    public Permission PermissionName
    {
        get
        {
            if (Enum.TryParse(PermissionNameString, out Permission result))
            {
                return result;
            }

            return Permission.Unknown;
        }
    }

    [JsonProperty]
    public string? PermissionType { get; set; }

    [JsonProperty]
    public string? TargetId { get; set; }
}