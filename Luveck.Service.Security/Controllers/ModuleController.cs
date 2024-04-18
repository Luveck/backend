using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Route("api/Module")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    [ApiExplorerSettings(GroupName = "ApiSecurityModule")]
    public class ModuleController : Controller
    {
        public IModuleRepository _moduleRepository;
        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(ResponseModel<List<RoleResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetModules()
        {
            List<ModuleResponseDto> result = await _moduleRepository.GetModules();
            var response = new ResponseModel<List<ModuleResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateModule")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> CreateModule(string name)
        {
            var module = await _moduleRepository.Insert(name);
            var response = new ResponseModel<GeneralResponseDto>()
            {
                IsSuccess = module.Code == "201" ? true : false,
                Messages = module.Message,
                Result = module,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteModule")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> DeleteModule(string name)
        {
            var module = await _moduleRepository.delete(name);
            return Ok(new
            {
                Modulo= name,
                Eliminado = module
            }
            );
        }
    }
}
