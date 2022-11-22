using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Controllers
{
    [Authorize]
    [Route("api/Purchase")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminPurchase")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class PurchaseController : ControllerBase
    {
        public IPurchaseRepository _purchase;
        public PurchaseController(IPurchaseRepository purchase)
        {
            _purchase = purchase;
        }
        [HttpGet]
        [Route("GetPurchases")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PurchaseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPurchases()
        {
            var purchases = await _purchase.GetPurchases();
            return Ok(purchases);
        }

        [HttpGet]
        [Route("GetPurchaseByPharmacy")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PurchaseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPurchaseByPharmacy(int idPharmacy)
        {
            var purchases = await _purchase.GetPurchaseByPharmacy(idPharmacy);
            return Ok(purchases);
        }

        [HttpGet]
        [Route("GetPurchaseByNoPurchase")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PurchaseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPurchaseByNoPurchase(string PurchaseNo)
        {
            var purchases = await _purchase.GetPurchaseByNoPurchase(PurchaseNo);
            return Ok(purchases);
        }

        [HttpGet]
        [Route("GetPurchaseByClientID")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<PurchaseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPurchaseByClientID(string identification)
        {
            var purchases = await _purchase.GetPurchaseByClientID(identification);
            return Ok(purchases);
        }
    }
}
