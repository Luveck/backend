using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminCountry")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class CountryController : ControllerBase
    {
        private ICountryRepository _countryRepository;
        private readonly IHeaderClaims _headerClaims;

        public CountryController(ICountryRepository countryRepository, IHeaderClaims headerClaims)
        {
            _countryRepository = countryRepository;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetCountries")]
        [ProducesResponseType(typeof(ResponseModel<List<CountryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountries()
        {
            List<CountryDto> result = await _countryRepository.GetCountries();
            var response = new ResponseModel<List<CountryDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCountryById")]
        [ProducesResponseType(typeof(ResponseModel<CountryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContryById(int Id)
        {
            CountryDto result = await _countryRepository.GetCountry(Id);
            var response = new ResponseModel<CountryDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCountryByName")]
        [ProducesResponseType(typeof(ResponseModel<CountryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountryByName(string name)
        {
            CountryDto result = await _countryRepository.GetCountryName(name);
            var response = new ResponseModel<CountryDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateCountry")]
        [ProducesResponseType(typeof(ResponseModel<CountryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCountry(CountryDto countryCreateUpdateDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            countryCreateUpdateDto.CreationDate = DateTime.Now;
            countryCreateUpdateDto.CreateBy = user;
            countryCreateUpdateDto.UpdateDate = DateTime.Now;
            countryCreateUpdateDto.UpdateBy = user;

            CountryDto result = await _countryRepository.CreateCountry(countryCreateUpdateDto);
            var response = new ResponseModel<CountryDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateCountry")]
        [ProducesResponseType(typeof(ResponseModel<CountryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCountry(CountryDto countryCreateUpdateDto)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            countryCreateUpdateDto.UpdateDate = DateTime.Now;
            countryCreateUpdateDto.UpdateBy = user;

            CountryDto result = await _countryRepository.UpdateCountry(countryCreateUpdateDto);
            var response = new ResponseModel<CountryDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteCountry")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            bool result = await _countryRepository.DeleteCountry(id, user);
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

