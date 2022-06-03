using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public bool State { get; set; }
    }
}
