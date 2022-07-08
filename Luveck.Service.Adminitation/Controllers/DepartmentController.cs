using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminDepartment")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepository _departmentRepositoy;

        public DepartmentController(IDepartmentRepository departmentRepositoy)
        {
            _departmentRepositoy = departmentRepositoy;
        }

        [HttpGet]
        [Route("GetDepartment")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<DepartmentsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _departmentRepositoy.GetDepartments();
            return Ok(countries);
        }

        /// <summary>
        /// Metodo para obtener los departamentos por Id
        /// </summary>
        /// <param name="Id">Id de la tabla se envia en el request</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartmentById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(DepartmentsDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContryById(int Id)
        {
            var countries = await _departmentRepositoy.GetDepartment(Id);
            return Ok(countries);
        }
    }
}
