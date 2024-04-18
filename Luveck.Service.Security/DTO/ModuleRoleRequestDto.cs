using System;
using System.Collections.Generic;

namespace Luveck.Service.Security.DTO
{
    public class ModuleRoleRequestDto
    {
        public string rolId { get; set; }
        public List<Int64> moduleId { get; set; }
    }
}
