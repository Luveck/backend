using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminCity")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class CityController : ControllerBase
    {
        public ICityRepository _cityRepository;
        private readonly IHeaderClaims _headerClaims;

        public CityController(ICityRepository cityRepository, IHeaderClaims headerClaims)
        {
            _cityRepository = cityRepository;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetCities")]
        [ProducesResponseType(typeof(ResponseModel<List<CityResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCities()
        {
            List<CityResponseDto> result = await _cityRepository.GetCities();
            var response = new ResponseModel<List<CityResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCitiesByDepartment")]
        [ProducesResponseType(typeof(ResponseModel<List<CityResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCitiesByDepartment(int departmentid)
        {
            List<CityResponseDto> result = await _cityRepository.GetCitiesByDepartment(departmentid);
            var response = new ResponseModel<List<CityResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCityByName")]
        [ProducesResponseType(typeof(ResponseModel<CityResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCityByName(string name)
        {
            CityResponseDto result = await _cityRepository.GetCityByName(name);
            var response = new ResponseModel<CityResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCityById")]
        [ProducesResponseType(typeof(ResponseModel<CityResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCityById(int id)
        {
            CityResponseDto result = await _cityRepository.GetCityById(id);
            var response = new ResponseModel<CityResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateCity")]
        [ProducesResponseType(typeof(ResponseModel<CityResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCity(CityRequestDto cityDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            CityResponseDto result = await _cityRepository.CreateCity(cityDto, user);
            var response = new ResponseModel<CityResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateCity")]
        [ProducesResponseType(typeof(ResponseModel<CityResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCity(CityRequestDto cityDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            CityResponseDto result = await _cityRepository.UpdateCity(cityDto, user);
            var response = new ResponseModel<CityResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteCity")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            bool result = await _cityRepository.DeleteCity(id, user);
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
