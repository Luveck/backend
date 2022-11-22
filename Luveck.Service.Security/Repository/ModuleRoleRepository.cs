
using Luveck.Service.Security.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.UnitWork;
using Luveck.Service.Security.DTO;
using Luveck.Service.Security.Models;

namespace Luveck.Service.Security.Repository
{
    public class ModuleRoleRepository : IModuleRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ModuleRoleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ModuleRoleResponseDto>> GetModulesByRoles()
        {
            List<ModuleRoleResponseDto> lst = await (from mr in _unitOfWork.RoleModuleRepository.AsQueryable() 
                                                     join m in this._unitOfWork.ModuleRepository.AsQueryable() on mr.module.Id equals m.Id
                                                     join r in _unitOfWork.RoleRepository.AsQueryable() on mr.role.Id equals r.Id
                                                     select new ModuleRoleResponseDto()
                                                     {
                                                         idModule = mr.module.Id,
                                                         idRole= mr.role.Id,
                                                         moduleName = m.name,
                                                         roleName = r.Name
                                                     }).ToListAsync();

            return lst;                        
        }

        public async Task<List<ModuleRoleResponseDto>> UpdateModulesByRole(List<ModuleRoleRequestDto> moduleRoleDtos)
        {
            var table = _unitOfWork.RoleModuleRepository.AsQueryable();
            try
            {
                _unitOfWork.RoleModuleRepository.Delete(table);
                await _unitOfWork.SaveAsync();

                List<RoleModule> roleModules= new List<RoleModule>();

                foreach (var item in moduleRoleDtos)
                {
                    var role = await _unitOfWork.RoleRepository.FirstOrDefaultNoTracking(x => x.Id.Equals(item));
                    foreach (var module in item.moduleId)
                    {
                        var mod = await _unitOfWork.ModuleRepository.FirstOrDefaultNoTracking(x => x.Id == module);
                        roleModules.Add(new RoleModule()
                        {
                            module = mod,
                            role = role
                        });
                    }
                }

                await _unitOfWork.RoleModuleRepository.InsertRangeAsync(roleModules);

                List<ModuleRoleResponseDto> lst = await (from mr in _unitOfWork.RoleModuleRepository.AsQueryable()
                                                         join m in this._unitOfWork.ModuleRepository.AsQueryable() on mr.module.Id equals m.Id
                                                         join r in _unitOfWork.RoleRepository.AsQueryable() on mr.role.Id equals r.Id
                                                         select new ModuleRoleResponseDto()
                                                         {
                                                             idModule = mr.module.Id,
                                                             idRole = mr.role.Id,
                                                             moduleName = m.name,
                                                             roleName = r.Name
                                                         }).ToListAsync();

                return lst;
            }
            catch (System.Exception Ex)
            {

                throw Ex;
            }

            return null;
        }
    }
}
