using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Luveck.Service.Administration.DTO.Response;
using System.Net;
using Luveck.Service.Administration.DTO;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPatology")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class PatologyController : ControllerBase
    {
        public IPatologyRepository Patology;
        private readonly IHeaderClaims _headerClaims;

        public PatologyController(IPatologyRepository patology, IHeaderClaims headerClaims)
        {
            Patology = patology;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetPatologies")]
        [ProducesResponseType(typeof(ResponseModel<List<PatologyResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPatologies()
        {
            List<PatologyResponseDto> result = await Patology.GetPatologies();
            var response = new ResponseModel<List<PatologyResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPatologyById")]
        [ProducesResponseType(typeof(ResponseModel<PatologyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPatologyById(int Id)
        {
            PatologyResponseDto result = await Patology.GetPatologyById(Id);
            var response = new ResponseModel<PatologyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPatologyByName")]
        [ProducesResponseType(typeof(ResponseModel<PatologyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPatologyByName(string name)
        {
            PatologyResponseDto result = await Patology.GetPatologyByName(name);
            var response = new ResponseModel<PatologyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreatePatology")]
        [ProducesResponseType(typeof(ResponseModel<PatologyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePatology(PatologyRequestDto patology)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            PatologyResponseDto result = await Patology.CreatePatology(patology, user);
            var response = new ResponseModel<PatologyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdatePatology")]
        [ProducesResponseType(typeof(ResponseModel<PatologyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePatology(PatologyRequestDto patology)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            PatologyResponseDto result = await Patology.UpdatePatology(patology, user);
            var response = new ResponseModel<PatologyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeletePatology")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePatology(int Id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            bool result = await Patology.deletePatology(Id, user);
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
