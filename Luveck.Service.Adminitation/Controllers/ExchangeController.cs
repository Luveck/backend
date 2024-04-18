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
    [Route("api/Exchange")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminExchangeProduct")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class ExchangeController : ControllerBase
    {
        private readonly IHeaderClaims _headerClaims;
        public readonly IExchangeProduct exchangeProduct;

        public ExchangeController(IHeaderClaims headerClaims, IExchangeProduct exchangeProduct)
        {
            _headerClaims = headerClaims;
            this.exchangeProduct = exchangeProduct;
        }

        [HttpGet]
        [Route("GetProductAvailable")]
        [ProducesResponseType(typeof(ResponseModel<List<ExchangeProductAvailableResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductAvailable(string user)
        {
            List<ExchangeProductAvailableResponseDto> result = await exchangeProduct.getProductExchangeAvailable(user);
            var response = new ResponseModel<List<ExchangeProductAvailableResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("ExchangeProduct")]
        [ProducesResponseType(typeof(ResponseModel<List<ExchangeProductAvailableResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExchangeProduct(List<ExchangeRequestDto> request)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            List<ExchangeResponseDto> result = await exchangeProduct.ExchangeProduc(request, user);
            var response = new ResponseModel<List<ExchangeResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }
    }
}
