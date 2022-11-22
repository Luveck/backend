using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Luveck.Service.Security.Models
{
    [ExcludeFromCodeCoverage]
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool changePass { get; set; }
        public bool State { get; set; }
    }
}
