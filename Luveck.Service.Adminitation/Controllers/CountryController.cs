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
    [ApiExplorerSettings(GroupName = "ApiAdminCountry")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class CountryController : ControllerBase
    {
        private ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [Route("GetCountries")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<CountryDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryRepository.GetCountries();
            return Ok(countries);
        }

        [HttpGet]
        [Route("GetCountryById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContryById(int Id)
        {
            var countries = await _countryRepository.GetCountry(Id);
            return Ok(countries);
        }

        [HttpPost]
        [Route("CreateUpdateCountry")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUpdateCountry(CountryDto countryCreateUpdateDto, string user)
        {
            if (countryCreateUpdateDto.Id > 0)
            {
                countryCreateUpdateDto.UpdateDate = DateTime.Now;
                countryCreateUpdateDto.UpdateBy = user;
            }
            else
            {
                countryCreateUpdateDto.CreationDate = DateTime.Now;
                countryCreateUpdateDto.CreateBy = user;
                countryCreateUpdateDto.UpdateDate = DateTime.Now;
                countryCreateUpdateDto.UpdateBy = user;

                var country = await _countryRepository.GetCountryName(countryCreateUpdateDto.Name);
                if (country != null)
                {
                    return Ok( new
                    {
                        Pais = country
                    });
                }
            }


            CountryDto countryDto = await _countryRepository.CreateUpdateCountry(countryCreateUpdateDto);

            return Ok(countryDto);
        }         
    }
}
