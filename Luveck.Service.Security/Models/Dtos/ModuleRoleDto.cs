using System.Collections.Generic;

namespace Luveck.Service.Administration.Models.Dto
{
    public class ModuleRoleDto
    {
        public string RoleId { get; set; }
        public List<int> modulesId{ get; set; }
    }
}
