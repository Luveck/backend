using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Security.Models
{
    [Keyless]
    public class RoleModule
    {
        [ForeignKey("ModuleId")]
        public Module module { get; set; }
        [ForeignKey("RoleId")]
        public IdentityRole role { get; set; }
    }
}
