using Microsoft.AspNetCore.Identity;

namespace Web.Startup.Example.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}