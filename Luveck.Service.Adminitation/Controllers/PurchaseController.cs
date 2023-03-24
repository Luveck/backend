using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Purchase")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPurchase")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class PurchaseController : ControllerBase
    {
        public IPurchaseRepository _purchase;
        private readonly IHeaderClaims _headerClaims;
        public PurchaseController(IPurchaseRepository purchase, IHeaderClaims headerClaims)
        {
            _purchase = purchase;
            _headerClaims = headerClaims;
        }


        [HttpGet]
        [Route("GetPurchases")]
        [ProducesResponseType(typeof(ResponseModel<List<PurshaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPurchases()
        {            
            var purchases = await _purchase.GetPurchases();
            var response = new ResponseModel<List<PurshaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPurchaseByPharmacy")]
        [ProducesResponseType(typeof(ResponseModel<List<PurshaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPurchaseByPharmacy(int idPharmacy)
        {
            var purchases = await _purchase.GetPurchaseByPharmacy(idPharmacy);
            var response = new ResponseModel<List<PurshaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPurchaseByNoPurchase")]
        [ProducesResponseType(typeof(ResponseModel<PurshaseResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPurchaseByNoPurchase(string PurchaseNo)
        {
            var purchases = await _purchase.GetPurchaseByNoPurchase(PurchaseNo);
            var response = new ResponseModel<PurshaseResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPurchaseByClientID")]
        [ProducesResponseType(typeof(ResponseModel<List<PurshaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPurchaseByClientID(string identification)
        {
            var purchases = await _purchase.GetPurchaseByClientID(identification);
            var response = new ResponseModel<List<PurshaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreatePurchase")]
        [ProducesResponseType(typeof(ResponseModel<PurshaseResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePurchase(PurchaseRequestDto purchaseDto )
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            var purchases = await _purchase.CreatePurchase(purchaseDto, user);
            var response = new ResponseModel<PurshaseResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdatePurchase")]
        [ProducesResponseType(typeof(ResponseModel<PurshaseResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePurchase(PurchaseRequestDto purchaseDto )
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            var purchases = await _purchase.UpdatePurchase(purchaseDto, user);
            var response = new ResponseModel<PurshaseResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }
    }
}
