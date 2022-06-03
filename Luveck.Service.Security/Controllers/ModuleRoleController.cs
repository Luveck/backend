using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Security.Data;
using Luveck.Service.Security.Models.Dtos;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Route("api/ModuleRoles")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "ApiSecurityModuleRole")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class ModuleRoleController : ControllerBase
    {
        public IModuleRoleRepository _moduleRoleRepository;

        public ModuleRoleController(IModuleRoleRepository moduleRoleRepository)
        {
            _moduleRoleRepository = moduleRoleRepository;
        }

        [HttpGet]
        [Route("GetModulesRoles")]
        [ProducesResponseType(200, Type = typeof(List<ListModuleRoleDto>))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> getRoles()
        {
            var mdoulesRole = _moduleRoleRepository.GetModulesByRoles();
            return Ok(new 
            {
                mdoulesRole 
            }
            );
        }

        [HttpPost]
        [Route("UpdateModuleRole")]        
        [ProducesResponseType(200, Type = typeof(List<ListModuleRoleDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateModulesRole(List<ModuleRoleDto> moduleRoleDto)
        {
            return Ok(_moduleRoleRepository.UpdateModulesByRole(moduleRoleDto));
        }
    }
}
