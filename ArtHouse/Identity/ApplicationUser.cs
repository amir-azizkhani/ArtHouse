using ArtHouse.Models;
using Microsoft.AspNetCore.Identity;

namespace ArtHouse.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Cart? Cart { get; set; }
    }
}