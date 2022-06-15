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
        public ModuleRoleRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListModuleRoleDto>> GetModulesByRoles()
        {
            return (from mr in _db.RoleModules
                    join r in _db.Roles on mr.role.Id equals r.Id
                    join m in _db.Modules on mr.module.Id equals m.Id
                    select (
                    new ListModuleRoleDto { idRole = r.Id, roleName = r.Name, idModule = m.Id, moduleName = m.name })).ToList();
        }

        public async Task<List<ListModuleRoleDto>> UpdateModulesByRole(List<ModuleRoleDto> moduleRoleDtos)
        {
            var table = (from rm in _db.RoleModules select rm).ToList();

            foreach (var row in table)
            {
                _db.RoleModules.Remove(row);
            }
            await _db.SaveChangesAsync();

            foreach (ModuleRoleDto roleModule in moduleRoleDtos)
            {
                var role = await _db.Roles.FirstOrDefaultAsync(x=> x.Id == roleModule.RoleId);
                foreach (var id in roleModule.RoleId)
                {
                    var modulo = await _db.Modules.FirstOrDefaultAsync(x => x.Id == id);
                    _db.RoleModules.Add(new RoleModule { role = role, module = modulo });
                }
                await _db.SaveChangesAsync();
            }
            return (from mr in _db.RoleModules
                    join r in _db.Roles on mr.role.Id equals r.Id
                    join m in _db.Modules on mr.module.Id equals m.Id
                    select (
                    new ListModuleRoleDto { idRole = r.Id, roleName = r.Name, idModule = m.Id, moduleName = m.name })).ToList();
        }
    }
}
