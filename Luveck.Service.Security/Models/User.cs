using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool changePass { get; set; }
        public bool State { get; set; }
    }
}
