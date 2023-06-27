using Microsoft.AspNetCore.Identity;

namespace Blazor.Startup.Example.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
