using AutoMapper;
using Luveck.Service.Security.Data;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Luveck.Service.Administration.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Luveck.Service.Security.Models.Dtos;

namespace Luveck.Service.Security.Repository
{
    public class ModuleRoleRepository : IModuleRoleRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;
        public ModuleRoleRepository( AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListModuleRoleDto>> GetModulesByRoles()
        {
            try
            {
                List<RoleModule> roleModules = await _db.RoleModules.ToListAsync();
                return _mapper.Map<List<ListModuleRoleDto>>(roleModules);
            }
            catch (System.Exception ex)
            {
                var data = ex.Data;
                return null;
            }
          
        }

        public async Task<List<ListModuleRoleDto>> UpdateModulesByRole(List<ModuleRoleDto> moduleRoleDtos)
        {
            foreach (ModuleRoleDto roleModule in moduleRoleDtos)
            {
                IEnumerable<RoleModule> moduleRole = await _db.RoleModules.Where(x => x.role.Id == roleModule.RoleId).ToListAsync();
                _db.RoleModules.RemoveRange(moduleRole);
                await _db.SaveChangesAsync();

                IdentityRole role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == roleModule.RoleId);
                foreach (int moduleId in roleModule.modulesId)
                {
                    _db.RoleModules.Add(new RoleModule { 
                        module = await _db.modules.FirstOrDefaultAsync(m => m.Id == moduleId), 
                        role = role});
                }
                await _db.SaveChangesAsync();
            }
            List<RoleModule> roleModules = await _db.RoleModules.ToListAsync();
            return _mapper.Map<List<ListModuleRoleDto>>(roleModules);
        }
    }
}
