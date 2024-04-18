using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IModuleRoleRepository
    {
        Task<List<ModuleRoleResponseDto>> GetModulesByRoles();
        Task<List<ModuleRoleResponseDto>> UpdateModulesByRole(List<ModuleRoleRequestDto> moduleRoleDtos);
    }
}
