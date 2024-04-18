using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Report")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiReport")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class ReportController : ControllerBase
    {
        private readonly IHeaderClaims _headerClaims;
        private readonly IReportService _reportService;

        public ReportController(IHeaderClaims headerClaims, IReportService reportService)
        {
            _headerClaims = headerClaims;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("GetPurchaseByUser")]
        [ProducesResponseType(typeof(ResponseModel<List<PurchaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPurchaseByUser(string userId, DateTime start, DateTime end)
        {
            var PurchaseByUser = await _reportService.GetPurchaseByUser(userId, start, end);
            var response = new ResponseModel<List<PurchaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = PurchaseByUser,
            };
            return Ok(response);
        }


        /// pending
        [HttpGet]
        [Route("GetReportGeneral")]
        [ProducesResponseType(typeof(ResponseModel<List<PurchaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReportGeneral(string userId, DateTime start, DateTime end)
        {
            var PurchaseByUser = await _reportService.GetPurchaseByUser(userId, start, end);
            var response = new ResponseModel<List<PurchaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = PurchaseByUser,
            };
            return Ok(response);
        }
        
    }
}
