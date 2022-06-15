using Microsoft.AspNetCore.Identity;

namespace Luveck.Service.Security.Models.Dtos
{
    public class ListModuleRoleDto
    {
        public string idRole { get; set; }
        public string roleName { get; set; }
        public int idModule { get; set; }
        public string moduleName { get; set; }
    }
}
