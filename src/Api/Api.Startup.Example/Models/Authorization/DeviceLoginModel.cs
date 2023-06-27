using System.ComponentModel.DataAnnotations;

namespace Api.Startup.Example.Models.Authorization;

public class DeviceLoginModel
{
    public Guid DeviceId { get; set; }

    [MaxLength(256)]
    public string DeviceLogin { get; set; }

    [MaxLength(256)]
    public string DeviceSecret { get; set; }
}