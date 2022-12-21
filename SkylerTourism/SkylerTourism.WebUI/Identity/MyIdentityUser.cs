using Microsoft.AspNetCore.Identity;

namespace SkylerTourism.WebUI.Identity
{
    public class MyIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
