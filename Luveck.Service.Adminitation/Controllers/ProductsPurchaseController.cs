using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
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
    [Route("api/ProductPurchase")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminProductPurchase")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class ProductsPurchaseController : ControllerBase
    {
        private readonly IHeaderClaims _headerClaims;
        private readonly IProductPurchaseRepository _productPurchaseRepository;

        public ProductsPurchaseController(IHeaderClaims headerClaims, IProductPurchaseRepository productPurchaseRepository)
        {
            _headerClaims = headerClaims;
            _productPurchaseRepository = productPurchaseRepository;
        }

        [HttpGet]
        [Route("GetProductByPurchase")]
        [ProducesResponseType(typeof(ResponseModel<List<ProductPruchaseResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductByPurchase(int purchaseId)
        {
            var purchases = await _productPurchaseRepository.getProductsByPurchase(purchaseId);
            var response = new ResponseModel<List<ProductPruchaseResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = purchases,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(ProductPurchaseRequestDto request)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            var purchases = await _productPurchaseRepository.DeleteProductsPurchase(request, user);
            var response = new ResponseModel<string>()
            {
                IsSuccess = purchases,
                Messages = "",
                Result = "",
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("AddUpdateProduct")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateProduct(ProductPurchaseRequestDto request)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            var purchases = await _productPurchaseRepository.CreateUpdateProductPurchase(request, user);
            var response = new ResponseModel<string>()
            {
                IsSuccess = purchases.Equals("200") ? true : false,
                Messages = "",
                Result = "",
            };
            return Ok(response);
        }
    }
}
