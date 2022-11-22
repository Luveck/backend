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
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminProduct")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class ProductController : ControllerBase
    {
        public IProductRepository _product;

        public ProductController(IProductRepository productRepository)
        {
            _product = productRepository;
        }

        [HttpGet]
        [Route("GetProducts")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _product.GetProducts();
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProductsById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts(int id)
        {
            var product = await _product.GetProduct(id);
            return Ok(product);
        }

        [HttpGet]
        [Route("GetProductsByCategory")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsByCategory(int idCategory)
        {
            var products = await _product.GetProductsByCategory(idCategory);
            return Ok(products);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var product = await _product.deleteProduct(Id);
            return Ok(product);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto, string user)
        {
            productDto.UpdateBy = user;
            ProductDto product = await _product.CreateUpdateProduct(productDto);
            return Ok(product);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct(ProductDto productDto, string user)
        {
            if(string.IsNullOrEmpty(productDto.Name) || string.IsNullOrEmpty(user))
            {
                return BadRequest("No puede enviar el nombre y/o el usuario vacio.");
            }
            productDto.UpdateBy = user;
            ProductDto product = await _product.CreateUpdateProduct(productDto);
            return Ok(product);
        }
    }
}
