using Microsoft.AspNetCore.Identity;

namespace Luveck.Service.Security.Models.Dtos
{
    public class ListModuleRoleDto
    {
        public IdentityRole rol { get; set; }
        public Module module { get; set; }
    }
}
