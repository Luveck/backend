using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Luveck.Service.Security.UnitWork
{
    public interface IUnitOfWork
    {
        #region Repository
        Repository<User> UserRepository { get; }
        Repository<IdentityRole> RoleRepository { get; }
        Repository<Module> ModuleRepository { get; }
        Repository<RoleModule> RoleModuleRepository { get; }
        Task<int> SaveAsync();
        #endregion
    }
}
