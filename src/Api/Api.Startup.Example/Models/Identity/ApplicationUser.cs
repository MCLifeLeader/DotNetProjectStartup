using Microsoft.AspNetCore.Identity;

namespace Startup.Api.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}