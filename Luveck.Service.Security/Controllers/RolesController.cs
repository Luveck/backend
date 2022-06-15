using Luveck.Service.Security.Data;
using Luveck.Service.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult getRoles()
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
            var role= _db.Roles.FirstOrDefault(u => u.Id == id);
            int currentStamp = 0;
            if (role == null)
            {
                return BadRequest("Role no existe");
            }

            if(!Int32.TryParse(role.NormalizedName, out currentStamp))
            {
                return BadRequest("Este role no puede ser eliminado.");
            }
            await _roleManager.DeleteAsync(role);
            return Ok("El Role ha sido borrado.");
        }

        [HttpPost]
        [Route("CreateRole")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (await _roleManager.RoleExistsAsync(name))
            {
                return BadRequest("Role ya existe");
            }
            var result = await _roleManager.CreateAsync(new IdentityRole() { Name = name });

            if (result.Succeeded)
            {
                return Ok(new
                {
                    role = name,
                    result = result.Succeeded,
                    message = "El Rol ha sido creado."
                });
            }
            return BadRequest("Se ha presentado un error creando el role.");
        }

        [HttpPost]
        [Route("UpdateRole")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> UpdateRole(string id, string newName)
        {
            var role = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (role != null)
            {
                return BadRequest("Role no existe");
            }
            role.Name = newName.Trim();
            role.NormalizedName = newName.ToUpper().Trim();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    role = role,
                    result = result.Succeeded,
                    message = "El Rol ha sido actualizado."
                });
            }
            return BadRequest("Se ha presentado un error actualizando el role.");
        }
    }
}
