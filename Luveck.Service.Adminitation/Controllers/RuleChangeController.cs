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
    [Route("api/RuleChange")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiRuleChage")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class RuleChangeController : ControllerBase
    {
        public IProductChangeRuleRepository _productChangeRuleRepository;
        private readonly IHeaderClaims _headerClaims;

        public RuleChangeController( IProductChangeRuleRepository productChangeRuleRepository, IHeaderClaims headerClaims )
        {
            _productChangeRuleRepository = productChangeRuleRepository;
            _headerClaims= headerClaims;
        }

        [HttpGet]
        [Route("GetRules")]
        [ProducesResponseType(typeof(ResponseModel<List<ProductRuleChangeResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRules()
        {
            List<ProductRuleChangeResponseDto> result = await _productChangeRuleRepository.GetRules();
            var response = new ResponseModel<List<ProductRuleChangeResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateRule")]
        [ProducesResponseType(typeof(ResponseModel<ProductRuleChangeResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateRule(ProductChangeRuleRequestDto request)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            ProductRuleChangeResponseDto result = await _productChangeRuleRepository.AddRule(request, user); ;
            var response = new ResponseModel<ProductRuleChangeResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateRule")]
        [ProducesResponseType(typeof(ResponseModel<ProductRuleChangeResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRule(ProductChangeRuleRequestDto request)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            ProductRuleChangeResponseDto result = await _productChangeRuleRepository.UpdateRule(request, user); ;
            var response = new ResponseModel<ProductRuleChangeResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteRule")]
        [ProducesResponseType(typeof(ResponseModel<ProductRuleChangeResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRule(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            ProductRuleChangeResponseDto result = await _productChangeRuleRepository.DeleteRule(id, user); ;
            var response = new ResponseModel<ProductRuleChangeResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductsLanding")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<ProductsLandingPageResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsLanding()
        {
            List<ProductsLandingPageResponseDto> result = await _productChangeRuleRepository.getProducts();
            var response = new ResponseModel<List<ProductsLandingPageResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }


        [HttpGet]
        [Route("GetRuleById")]
        [ProducesResponseType(typeof(ResponseModel<ProductRuleChangeResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRuleById(int id)
        {
            ProductRuleChangeResponseDto result = await _productChangeRuleRepository.GetRuleById(id);
            var response = new ResponseModel<ProductRuleChangeResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }
    }
}
