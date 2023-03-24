using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Luveck.Service.Security.Utils.Exceptions;
using Luveck.Service.Security.Utils.Resource;
using System.Collections.Generic;
using Luveck.Service.Security.UnitWork;
using System.Linq;
using System.Data;
using Luveck.Service.Security.DTO;
using Luveck.Service.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace Luveck.Service.Security.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleRepository(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            this.roleManager = roleManager;
        }
        public async Task<GeneralResponseDto> CreateRole(string role, string user)
        {
            var exist = await _unitOfWork.RoleRepository.Find(x => x.Name.ToLower().Equals(role.ToLower()));

            if (exist != null) throw new BusinessException(GeneralMessage.RoleExist);

            try
            {
                Role roleNew = new Role()
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    state = true,
                    UpdateDate = DateTime.Now,
                    UpdateBy = user,
                    CreateBy = user,
                    CreationDate = DateTime.Now,
                };
                await _unitOfWork.RoleRepository.InsertAsync(roleNew);

                await _unitOfWork.SaveAsync();

                return new GeneralResponseDto() { Code = "201", Message = GeneralMessage.RoleCreate };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RoleResponseDto>> GetRoles()
        {

            var role = roleManager.Roles;
            List<RoleResponseDto> list = new List<RoleResponseDto>();
            foreach (var item in role)
            {
                list.Add(new RoleResponseDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }

            return list;
        }

        public async Task<GeneralResponseDto> UpdateRole(RoleRequestDto role, string user)
        {
            Role exist = await _unitOfWork.RoleRepository.Find(x => x.Name.ToUpper().Equals(role.RoleName.ToUpper()));
            if (exist != null) throw new BusinessException(GeneralMessage.RoleExist);
            
            exist = await _unitOfWork.RoleRepository.Find(x => x.Id == role.RoleId);
            if (exist == null) throw new BusinessException(GeneralMessage.RoleNoExist);

            try
            {
                exist.Name = role.RoleName;
                exist.NormalizedName = role.RoleName.ToUpper();
                exist.UpdateDate = DateTime.Now;
                exist.UpdateBy = user;
                exist.state = role.state;

                _unitOfWork.RoleRepository.Update(exist);
                await _unitOfWork.SaveAsync();

                return new GeneralResponseDto() { Code = "201", Message = GeneralMessage.RoleUpdated };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GeneralResponseDto> DeleteRole(string roleId, string user)
        {
            var exist = await _unitOfWork.RoleRepository.Find(x => x.Id == roleId);
            if (exist == null) throw new BusinessException(GeneralMessage.RoleNoExist);

            try
            {
                exist.state = false;
                exist.UpdateDate = DateTime.Now;
                exist.UpdateBy = user;

                _unitOfWork.RoleRepository.Update(exist);
                await _unitOfWork.SaveAsync();

                return new GeneralResponseDto() { Code = "201", Message = GeneralMessage.RoleUpdated };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
