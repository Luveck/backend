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
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPatology")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class PatologyController : ControllerBase
    {
        public IPatologyRepository Patology;

        public PatologyController(IPatologyRepository patology)
        {
            Patology = patology;
        }

        [HttpGet]
        [Route("GetPatologies")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PatologyDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatologies()
        {
            var patologies = await Patology.GetPatologies();
            return Ok(patologies);
        }

        [HttpGet]
        [Route("GetPatologyById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PatologyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatologyById(int Id)
        {
            var category = await Patology.GetPatology(Id);
            return Ok(category);
        }

        [HttpPost]
        [Route("CreatePatology")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PatologyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePatology(PatologyDto patologyDto, string user)
        {
            if(string.IsNullOrEmpty(user) || string.IsNullOrEmpty(patologyDto.Name))
            {
                return BadRequest("usuario o nombre patologia no pueden ser vacios.");
            }
            patologyDto.UpdateBy = user;
            PatologyDto patology = await Patology.CreateUpdatePatology(patologyDto);
            return Ok(patology);
        }

        [HttpPut]
        [Route("UpdatePatology")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PatologyDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatology(PatologyDto patologyDto, string user)
        {
            patologyDto.UpdateBy = user;
            PatologyDto category = await Patology.CreateUpdatePatology(patologyDto);
            return Ok(category);
        }

        [HttpDelete]
        [Route("DeletePatology")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatology(int Id, string user)
        {
            var patology = await Patology.deletePatology(Id, user);
            return Ok(patology);
        }
    }
}
