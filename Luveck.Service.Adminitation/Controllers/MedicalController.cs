using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Luveck.Service.Administration.Models;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Medical")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminMedical")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class MedicalController : ControllerBase
    {
        public IMedicalRepository _medical;

        public MedicalController(IMedicalRepository medical)
        {
            _medical = medical;
        }

        [HttpGet]
        [Route("GetMedicals")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MedicalDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedicals()
        {
            var medicals = await _medical.GetMedicals();
            return Ok(medicals);
        }

        [HttpGet]
        [Route("GetMedicalByPatolgy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MedicalDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedicalByPatolgy(int idPatology)
        {
            var medicals = await _medical.GetMedicalByPatolgy(idPatology);
            return Ok(medicals);
        }

        [HttpGet]
        [Route("GetMedicalByName")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MedicalDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedicalByName(string nameMedical)
        {
            var medicals = await _medical.GetMedicalByName(nameMedical);
            return Ok(medicals);
        }

        [HttpGet]
        [Route("GetMedical")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MedicalDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedical(int id)
        {
            var medical = await _medical.GetMedical(id);
            return Ok(medical);
        }

        [HttpPost]
        [Route("CreateMedical")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MedicalDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMedical(MedicalDto medicalDto)
        {
            medicalDto.UpdateDate = DateTime.Now;
            medicalDto.CreationDate = DateTime.Now;
            var medical = await _medical.CreateUpdateMedical(medicalDto);
            return Ok(medical);
        }

        [HttpPut]
        [Route("UpdateMedical")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MedicalDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMedical(MedicalDto medicalDto)
        {
            medicalDto.UpdateDate = DateTime.Now;
            medicalDto.CreationDate = DateTime.Now;
            var medical = await _medical.CreateUpdateMedical(medicalDto);
            return Ok(medical);
        }

        [HttpDelete]
        [Route("DeleteMedical")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMedical(int Id, string user)
        {
            var medical = await _medical.deleteMedical(Id, user);
            return Ok(medical);
        }
    }
}
