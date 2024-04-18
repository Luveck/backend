using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminDepartment")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepository _departmentRepositoy;
        private readonly IHeaderClaims _headerClaims;

        public DepartmentController(IDepartmentRepository departmentRepositoy, IHeaderClaims headerClaims)
        {
            _departmentRepositoy = departmentRepositoy;
            _headerClaims= headerClaims;
        }

        [HttpGet]
        [Route("GetDepartments")]
        [ProducesResponseType(typeof(ResponseModel<List<DepartmentResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDepartments()
        {
            List<DepartmentResponseDto> result = await _departmentRepositoy.GetDepartments();
            var response = new ResponseModel<List<DepartmentResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetDepartmentsByCountryId")]
        [ProducesResponseType(typeof(ResponseModel<List<DepartmentResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDepartmentsByCountryId(int idCountry)
        {
            List<DepartmentResponseDto> result = await _departmentRepositoy.GetDepartmentsByCountryId(idCountry);
            var response = new ResponseModel<List<DepartmentResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        /// <summary>
        /// Metodo para obtener los departamentos por Id
        /// </summary>
        /// <param name="Id">Id de la tabla se envia en el request</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartmentById")]
        [ProducesResponseType(typeof(ResponseModel<DepartmentResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDepartmentById(int Id)
        {
            DepartmentResponseDto result = await _departmentRepositoy.GetDepartmentById(Id);
            var response = new ResponseModel<DepartmentResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        /// <summary>
        /// Metodo para obtener los departamentos por nombre
        /// </summary>
        /// <param name="name">Id de la tabla se envia en el request</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartmentByName")]
        [ProducesResponseType(typeof(ResponseModel<DepartmentResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDepartmentByName(string name)
        {
            DepartmentResponseDto result = await _departmentRepositoy.GetDepartmentByName(name);
            var response = new ResponseModel<DepartmentResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateDepartment")]
        [ProducesResponseType(typeof(ResponseModel<DepartmentResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDepartment(DepartmentCreateUpdateRequestDto departmentDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            DepartmentResponseDto result = await _departmentRepositoy.CreateDepartment(departmentDto, user);
            var response = new ResponseModel<DepartmentResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateDepartment")]
        [ProducesResponseType(typeof(ResponseModel<DepartmentResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDepartment(DepartmentCreateUpdateRequestDto departmentDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            DepartmentResponseDto result = await _departmentRepositoy.UpdateDepartment(departmentDto, user);
            var response = new ResponseModel<DepartmentResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteDepartment")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            bool result = await _departmentRepositoy.DeleteDepartment(id, user);
            var response = new ResponseModel<string>()
            {
                IsSuccess = result,
                Messages = "",
                Result = "",
            };
            return Ok(response);
        }
    }
}
