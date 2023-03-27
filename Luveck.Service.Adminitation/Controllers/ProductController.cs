using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Administration.Utils.enums.Enums;

namespace Luveck.Service.Administration.Controllers
{
    //[Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminProduct")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class ProductController : ControllerBase
    {
        public IProductRepository _product;
        private readonly IHeaderClaims _headerClaims;

        public ProductController(IProductRepository productRepository, IHeaderClaims headerClaims)
        {
            _product = productRepository;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(ResponseModel<List<ProductResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductResponseDto> result = await _product.GetProducts();
            var response = new ResponseModel<List<ProductResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductsByCategory")]
        [ProducesResponseType(typeof(ResponseModel<List<ProductResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategory(int idCategory)
        {
            List<ProductResponseDto> result = await _product.GetProductsByCategory( idCategory );
            var response = new ResponseModel<List<ProductResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductById")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsById(int id)
        {
            ProductResponseDto result = await _product.GetProductById(id);
            var response = new ResponseModel<ProductResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductByName")]
        [ProducesResponseType(typeof(ResponseModel<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductByName(string name)
        {
            ProductResponseDto result = await _product.GetProductByName(name);
            var response = new ResponseModel<ProductResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductByBarcode")]
        [ProducesResponseType(typeof(ResponseModel<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            ProductResponseDto result = await _product.GetProductByBarcode(barcode);
            var response = new ResponseModel<ProductResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            bool result = await _product.deleteProduct(id, user); ;
            var response = new ResponseModel<string>()
            {
                IsSuccess = result,
                Messages = "",
                Result = "",
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(ResponseModel<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct( ProductRequestDto product)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            ProductResponseDto result = await _product.UpdateProduct(product, user);
            var response = new ResponseModel<ProductResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ResponseModel<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct(ProductRequestDto product)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            ProductResponseDto result = await _product.CreateProduct(product, user);
            var response = new ResponseModel<ProductResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteImg")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteImg(string fileName)
        {
            bool result = await _product.deleteImage(fileName); ;
            var response = new ResponseModel<string>()
            {
                IsSuccess = result,
                Messages = "",
                Result = "",
            };
            return Ok(response);
        }
    }
}
