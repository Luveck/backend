using Luveck.Service.Security.Data;
using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Security.Utils.enums.Enums;

namespace Luveck.Service.Security.Controllers
{
    [TypeFilter(typeof(CustomExceptionAttribute))]
    [ApiController]
    [Authorize]
    [Route("api/Roles")]
    [ApiExplorerSettings(GroupName = "ApiSecurityRoles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _role;
        private readonly IHeaderClaims _headerClaims;        

        public RolesController(IRoleRepository role, IHeaderClaims headerClaims)
        {
            _role = role;
            _headerClaims = headerClaims;
        }

        [Route("CreateRole")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateRole(string role)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            GeneralResponseDto result = await _role.CreateRole(role, user);

            var response = new ResponseModel<GeneralResponseDto>()
            {
                IsSuccess = result.Code == "201" ? true : false,
                Messages = result.Message,
                Result = result
            };
            return Ok(response);

        }

        /// <summary>
        /// Get roles by id
        /// </summary>
        /// <param></param>
        /// <returns>List RolesResponseDto</returns>       
        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(ResponseModel<List<RoleResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles()
        {
            var result = _role.GetRoles();
            var response = new ResponseModel<List<RoleResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result.Result,
            };
            return Ok(response);
        }

        /// <summary>
        /// Post to update role name in table role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Route("UpdateRole")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRole(RoleRequestDto role)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            GeneralResponseDto result = await _role.UpdateRole(role, user);

            var response = new ResponseModel<GeneralResponseDto>()
            {
                IsSuccess = result.Code == "201" ? true : false,
                Messages = result.Message,
                Result = result
            };
            return Ok(response);

        }

        /// <summary>
        /// Post to delete role name in table role, change state on table to false
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Route("DeleteRole")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            GeneralResponseDto result = await _role.DeleteRole(roleId, user);

            var response = new ResponseModel<GeneralResponseDto>()
            {
                IsSuccess = result.Code == "201" ? true : false,
                Messages = result.Message,
                Result = result
            };
            return Ok(response);

        }
    }
}
