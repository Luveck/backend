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
    [Route("api/Administrator")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminSBU")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class SBUController : ControllerBase
    {
        private readonly ISBURepository _SBURepository;

        //public SBUController(ISBURepository sBURepository)
        //{
        //    _SBURepository = sBURepository;
        //}

        //[HttpGet]
        //[Route("Getsbus")]
        //[AllowAnonymous]
        //[ProducesResponseType(200, Type = typeof(List<SBUDto>))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetCities()
        //{
        //    var SBUs = await _SBURepository.GetSBUs();
        //    return Ok(SBUs);
        //}

        //[HttpGet]
        //[Route("GetSBUById")]
        //[AllowAnonymous]
        //[ProducesResponseType(200, Type = typeof(SBUDto))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetContryById(int Id)
        //{
        //    var sbu = await _SBURepository.GetSBU(Id);
        //    return Ok(sbu);
        //}

        //[HttpPost]
        //[Route("CreateUpdateSBU")]
        //[AllowAnonymous]
        //[ProducesResponseType(200, Type = typeof(SBUDto))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CreateUpdateCountry(SBUDto sbuDto, string user)
        //{
        //    if (sbuDto.Id > 0)
        //    {
        //        sbuDto.UpdateDate = DateTime.Now;
        //        sbuDto.UpdateBy = user;
        //    }
        //    else
        //    {
        //        sbuDto.CreatedDate = DateTime.Now;
        //        sbuDto.CreateBy = user;
        //        sbuDto.UpdateDate = DateTime.Now;
        //        sbuDto.UpdateBy = user;
        //    }
        //    SBUDto SBUDto = await _SBURepository.CreateUpdateSBU(sbuDto);

        //    return Ok(SBUDto);
        //}
    }
}
