using System.ComponentModel.DataAnnotations;

namespace Startup.Common.Models.Authorization;

public class DeviceLoginModel
{
    public Guid DeviceId { get; set; }
    [MaxLength(256)]
    public string? DeviceLogin { get; set; }
    [MaxLength(256)]
    public string? DeviceSecret { get; set; }
}