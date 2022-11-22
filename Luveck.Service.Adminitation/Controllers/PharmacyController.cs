using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Pharmacy")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPharmacy")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class PharmacyController : ControllerBase
    {
        public IPharmacyRepository _pharmacy;

        public PharmacyController(IPharmacyRepository pharmacy)
        {
            _pharmacy = pharmacy;
        }

        [HttpGet]
        [Route("GetPharmacies")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PharmacyDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPharmacies()
        {
            var pharmacies = await _pharmacy.GetPharmacies();
            return Ok(pharmacies);
        }

        [HttpGet]
        [Route("GetPharmaciesByCity")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PharmacyDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPharmaciesByCity(int IdCity)
        {
            var pharmacies = await _pharmacy.GetPharmaciesByCity(IdCity);
            return Ok(pharmacies);
        }

        [HttpGet]
        [Route("GetPharmaciesByName")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PharmacyDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPharmaciesByName(string namePharmacy)
        {
            var pharmacies = await _pharmacy.GetPharmaciesByName(namePharmacy);
            return Ok(pharmacies);
        }

        [HttpGet]
        [Route("GetPharmacy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PharmacyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPharmacy(int id)
        {
            var pharmacy = await _pharmacy.GetPharmacy(id);
            return Ok(pharmacy);
        }

        [HttpPost]
        [Route("CreatePharmacy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PharmacyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePharmacy(PharmacyDto pharmacyDto)
        {
            PharmacyDto pharmacy = await _pharmacy.CreateUpdatePharmacy(pharmacyDto);
            return Ok(pharmacy);
        }

        [HttpPut]
        [Route("UpdatePharmacy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PatologyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePharmacy(PharmacyDto pharmacyDto)
        {
            pharmacyDto.UpdateDate = DateTime.Now;
            PharmacyDto pharmacy = await _pharmacy.CreateUpdatePharmacy(pharmacyDto);
            return Ok(pharmacy);
        }

        [HttpDelete]
        [Route("DeletePharmacy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePharmacy(int Id, string user)
        {
            var pharmacy = await _pharmacy.deletePharmacy(Id, user);
            return Ok(pharmacy);
        }
    }
}
