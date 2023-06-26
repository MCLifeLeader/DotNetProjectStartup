using Microsoft.AspNetCore.Identity;

namespace Api.Startup.Example.Model.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}