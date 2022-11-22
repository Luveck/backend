using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Luveck.Service.Security.Repository;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Data;
using System.Diagnostics.CodeAnalysis;

namespace Luveck.Service.Security.UnitWork
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {

        #region Attributes
        private readonly AppDbContext _contextSQL;
        #endregion Attributes

        #region Constructor
        public UnitOfWork(AppDbContext contextSQL)
        {
            _contextSQL = contextSQL;
        }
        #endregion Constructor


        #region Repository
        private Repository<User> userRepository;
        private Repository<IdentityRole> roleRepository;
        private Repository<Module> moduleRepository;
        private Repository<RoleModule> roleModuleRepository;
        public Repository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<User>(_contextSQL);
                }

                return this.userRepository;
            }
        }
        public Repository<IdentityRole> RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new Repository<IdentityRole>(_contextSQL);
                }

                return roleRepository;
            }
        }
        public Repository<Module> ModuleRepository
        {
            get
            {
                if (moduleRepository == null)
                {
                    moduleRepository = new Repository<Module>(_contextSQL);
                }

                return moduleRepository;
            }
        }
        public Repository<RoleModule> RoleModuleRepository
        {
            get
            {
                if (roleModuleRepository == null)
                {
                    roleModuleRepository = new Repository<RoleModule>(_contextSQL);
                }

                return roleModuleRepository;
            }
        }
        #endregion 


        public async Task<int> SaveAsync()
        {
            int result = 0;
            result = await _contextSQL.SaveChangesAsync();

            return result;
        }

    }
}
