using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Authorize]    
    [Route("api/ModuleRoles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiSecurityModuleRole")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class ModuleRoleController : ControllerBase
    {
        public IModuleRoleRepository _moduleRoleRepository;

        public ModuleRoleController(IModuleRoleRepository moduleRoleRepository)
        {
            _moduleRoleRepository = moduleRoleRepository;
        }

        [HttpGet]
        [Route("GetModulesRoles")]
        [ProducesResponseType(typeof(ResponseModel<List<ModuleRoleRequestDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> getRoles()
        {
            var mdoulesRole = await _moduleRoleRepository.GetModulesByRoles();
            var response = new ResponseModel<List<ModuleRoleResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = mdoulesRole,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateModuleRole")]
        [ProducesResponseType(typeof(ResponseModel<List<ModuleRoleRequestDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateModulesRole(List<ModuleRoleRequestDto> moduleRoleDto)
        {
            var mdoulesRole = await _moduleRoleRepository.UpdateModulesByRole(moduleRoleDto);
            var response = new ResponseModel<List<ModuleRoleResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = mdoulesRole,
            };
            return Ok(response);
        }
    }
}
