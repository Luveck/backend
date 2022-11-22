using Luveck.Service.Security.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IRole
    {
        Task<GeneralResponseDto> CreateRole(string role);
        Task<List<RoleResponseDto>> GetRoles();
    }
}
