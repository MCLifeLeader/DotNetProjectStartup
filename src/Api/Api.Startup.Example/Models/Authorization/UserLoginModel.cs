using System.ComponentModel.DataAnnotations;

namespace Api.Startup.Example.Models.Authorization;

public class UserLoginModel
{
    [MaxLength(256)]
    public string Username { get; set; }

    [MaxLength(256)]
    public string Password { get; set; }

    [MaxLength(256)]
    public string DisplayName { get; set; }
}