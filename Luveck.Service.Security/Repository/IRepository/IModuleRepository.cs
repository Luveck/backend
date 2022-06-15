using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IModuleRepository
    {
        Task<IEnumerable<ModuleDto>> GetModules();
        Task<Module> Insert(string name);
        Task<bool> delete(string name);
    }
}
