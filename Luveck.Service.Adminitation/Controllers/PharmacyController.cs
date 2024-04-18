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
using static Luveck.Service.Administration.Utils.enums.Enums;
using Luveck.Service.Administration.DTO;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Pharmacy")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPharmacy")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class PharmacyController : ControllerBase
    {
        public IPharmacyRepository _pharmacy;
        private readonly IHeaderClaims _headerClaims;

        public PharmacyController(IPharmacyRepository pharmacy, IHeaderClaims headerClaims)
        {
            _pharmacy = pharmacy;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetPharmacies")]
        [ProducesResponseType(typeof(ResponseModel<List<PharmacyResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPharmacies()
        {
            List<PharmacyResponseDto> result = await _pharmacy.GetPharmacies();
            var response = new ResponseModel<List<PharmacyResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPharmaciesByCity")]
        [ProducesResponseType(typeof(ResponseModel<List<PharmacyResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPharmaciesByCity(int IdCity)
        {
            List<PharmacyResponseDto> result = await _pharmacy.GetPharmaciesByCity(IdCity);
            var response = new ResponseModel<List<PharmacyResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPharmaciesByName")]
        [ProducesResponseType(typeof(ResponseModel<List<PharmacyResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPharmaciesByName(string namePharmacy)
        {
            List<PharmacyResponseDto> result = await _pharmacy.GetPharmaciesByName(namePharmacy);
            var response = new ResponseModel<List<PharmacyResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPharmacy")]
        [ProducesResponseType(typeof(ResponseModel<PharmacyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPharmacy(int id)
        {
            PharmacyResponseDto result = await _pharmacy.GetPharmacy(id);
            var response = new ResponseModel<PharmacyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreatePharmacy")]
        [ProducesResponseType(typeof(ResponseModel<PharmacyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePharmacy(PharmacyRequestDto pharmacyDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            PharmacyResponseDto result = await _pharmacy.CreatePharmacy(pharmacyDto, user);
            var response = new ResponseModel<PharmacyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdatePharmacy")]
        [ProducesResponseType(typeof(ResponseModel<PharmacyResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePharmacy(PharmacyRequestDto pharmacyDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            PharmacyResponseDto result = await _pharmacy.UpdatePharmacy(pharmacyDto, user);
            var response = new ResponseModel<PharmacyResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeletePharmacy")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePharmacy(int Id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            bool result = await _pharmacy.deletePharmacy(Id, user);
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
