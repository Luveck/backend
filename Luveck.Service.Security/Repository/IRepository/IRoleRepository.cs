using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IRoleRepository
    {
        Task<GeneralResponseDto> CreateRole(string role, string user);
        Task<GeneralResponseDto> UpdateRole(RoleRequestDto role, string user);
        Task<List<RoleResponseDto>> GetRoles();
        Task<GeneralResponseDto> DeleteRole(string roleId, string user);
    }
}
