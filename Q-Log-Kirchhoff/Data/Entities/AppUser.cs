using Microsoft.AspNetCore.Identity;


namespace MVC.Data.Entities
{
    /// <summary>
    /// The App specific version of a user based on the IdentitySystem.
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string AuthorizationGroup { get; set; }


    }
}
