using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Luveck.Service.Administration.DTO.Response;
using System.Net;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.DTO;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Medical")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminMedical")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class MedicalController : ControllerBase
    {
        public IMedicalRepository _medical;
        private readonly IHeaderClaims _headerClaims;

        public MedicalController(IMedicalRepository medical, IHeaderClaims headerClaims)
        {
            _medical = medical;
            _headerClaims= headerClaims;
        }

        [HttpGet]
        [Route("GetMedicals")]
        [ProducesResponseType(typeof(ResponseModel<List<MedicalResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMedicals()
        {
            List<MedicalResponseDto> result = await _medical.GetMedicals();
            var response = new ResponseModel<List<MedicalResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetMedicalByPatolgy")]
        [ProducesResponseType(typeof(ResponseModel<List<MedicalResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMedicalByPatolgy(int idPatology)
        {
            List<MedicalResponseDto> result = await _medical.GetMedicalByPatolgy(idPatology);
            var response = new ResponseModel<List<MedicalResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetMedicalsByName")]
        [ProducesResponseType(typeof(ResponseModel<List<MedicalResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMedicalByName(string nameMedicaly)
        {
            List<MedicalResponseDto> result = await _medical.GetMedicalByName(nameMedicaly);
            var response = new ResponseModel<List<MedicalResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetMedicalById")]
        [ProducesResponseType(typeof(ResponseModel<MedicalResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMedicalById(int id)
        {
            MedicalResponseDto result = await _medical.GetMedical(id);
            var response = new ResponseModel<MedicalResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateMedical")]
        [ProducesResponseType(typeof(ResponseModel<MedicalResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMedical(MedicalRequestDto requestDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            MedicalResponseDto result = await _medical.CreateMedical(requestDto, user);
            var response = new ResponseModel<MedicalResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateMedical")]
        [ProducesResponseType(typeof(ResponseModel<MedicalResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMedical(MedicalRequestDto requestDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            MedicalResponseDto result = await _medical.UpdateMedical(requestDto, user);
            var response = new ResponseModel<MedicalResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteMedical")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMedical(int Id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            bool result = await _medical.deleteMedical(Id, user);
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
