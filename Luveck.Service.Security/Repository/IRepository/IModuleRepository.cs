using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IModuleRepository
    {
        Task<List<ModuleResponseDto>> GetModules();
        Task<GeneralResponseDto> Insert(string name);
        Task<bool> delete(string name);
    }
}
