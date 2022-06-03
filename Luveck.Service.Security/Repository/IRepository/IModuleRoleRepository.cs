using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IModuleRoleRepository
    {
        Task<IEnumerable<ListModuleRoleDto>> GetModulesByRoles();
        Task<List<ListModuleRoleDto>> UpdateModulesByRole(List<ModuleRoleDto> moduleRoleDtos);
    }
}
