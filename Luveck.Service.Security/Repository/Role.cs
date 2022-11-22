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

namespace Luveck.Service.Security.Repository
{
    public class Role : IRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public Role(RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponseDto> CreateRole(string role)
        {
            var roleExist = _roleManager.RoleExistsAsync(role.Trim());

            if (roleExist != null) throw new BusinessException(GeneralMessage.RoleExist);

            try
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                });
                return new GeneralResponseDto() { Code = "201", Message = GeneralMessage.RoleCreate };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RoleResponseDto>> GetRoles()
        {
            List<RoleResponseDto> lstRoles = _unitOfWork.RoleRepository.AsQueryable().Select(y => new RoleResponseDto()
            {
                Name = y.Name,
                Id = y.Id
            }).ToList();

            return lstRoles;
        }
    }
}
