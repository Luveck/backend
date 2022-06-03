using Luveck.Service.Security.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Authorize]
    [Route("api/Roles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiSecurityRoles")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(AppDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(200, Type = typeof(IdentityRole))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> getRoles()
        {
            var roles = _db.Roles.ToList();
            return Ok(roles) ;
        }

        [HttpGet]
        [Route("GetRolById")]
        [ProducesResponseType(200, Type = typeof(IdentityRole))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult getRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Debe enviar el id del rol");
            }
            var role = _db.Roles.FirstOrDefault(u => u.Id == id); ;
            return Ok(role);
        }

        [HttpDelete]
        [Route("DeleteRole")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Delete(string id)
        {
            var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return BadRequest("Rol no existe");
            }

            await _roleManager.DeleteAsync(objFromDb);
            return Ok("El Rol ha sido borrado.");
        }
    }
}
