using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminCity")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class CityController : ControllerBase
    {
        public ICityRepository _cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        [Route("GetCities")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<CityDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityRepository.GetCities();
            return Ok(cities);
        }

        [HttpGet]
        [Route("GetCityById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContryById(int Id)
        {
            var city = await _cityRepository.GetCity(Id);
            return Ok(city);
        }

        [HttpPost]
        [Route("CreateUpdateCity")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CountryCreateUpdateDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUpdateCountry(CityDto cityCreateUpdateDto, string user)
        {
            if (cityCreateUpdateDto.Id > 0)
            {
                cityCreateUpdateDto.UpdateDate = DateTime.Now;
                cityCreateUpdateDto.UpdateBy = user;
            }
            else
            {
                cityCreateUpdateDto.CreationDate = DateTime.Now;
                cityCreateUpdateDto.CreateBy = user;
                cityCreateUpdateDto.UpdateDate = DateTime.Now;
                cityCreateUpdateDto.UpdateBy = user;
            }
            CityDto cityDto = await _cityRepository.CreateUpdateCity(cityCreateUpdateDto);

            return Ok(cityDto);
        }
    }
}
