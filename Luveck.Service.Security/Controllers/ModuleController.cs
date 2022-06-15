using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Route("api/Module")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiSecurityModule")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class ModuleController : Controller
    {
        public IModuleRepository _moduleRepository;
        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        [Route("GetModules")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> getRoles()
        {
            var module = await _moduleRepository.GetModules();
            return Ok(new
            {
                modulos= module
            }
            );
        }

        [HttpPost]
        [Route("PostCreateModule")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> PostCreateModule(string name)
        {
            var module = await _moduleRepository.Insert(name);
            return Ok(new
            {
                module= module
            }
            );
        }

        [HttpDelete]
        [Route("PostDeleteModule")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> PostDeleteModule(string name)
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
