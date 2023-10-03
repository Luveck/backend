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
    [Authorize]
    [Route("api/Administration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAdminCategory")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class CategoryController : ControllerBase
    {
        public ICategoryRepository _category;
        private readonly IHeaderClaims _headerClaims;

        public CategoryController(ICategoryRepository category, IHeaderClaims headerClaims)
        {
            _category = category;
            _headerClaims = headerClaims;
        }

        [HttpGet]
        [Route("GetCategories")]
        [ProducesResponseType(typeof(ResponseModel<List<CategoryResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategories()
        {
            List<CategoryResponseDto> result = await _category.GetCategories();
            var response = new ResponseModel<List<CategoryResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCategoryById")]
        [ProducesResponseType(typeof(ResponseModel<CategoryResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            CategoryResponseDto result = await _category.GetCategoryById(Id); ;
            var response = new ResponseModel<CategoryResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCategoryByName")]
        [ProducesResponseType(typeof(ResponseModel<CategoryResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            CategoryResponseDto result = await _category.GetCategoryByName(name); ;
            var response = new ResponseModel<CategoryResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateCategory")]
        [ProducesResponseType(typeof(ResponseModel<CategoryResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCategory(CategoryRequestDto category)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            CategoryResponseDto result = await _category.CreateCategory(category, user); ;
            var response = new ResponseModel<CategoryResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateCategory")]
        [ProducesResponseType(typeof(ResponseModel<CategoryResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategory(CategoryRequestDto category)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            CategoryResponseDto result = await _category.UpdateCategory(category, user); ;
            var response = new ResponseModel<CategoryResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [ProducesResponseType(typeof(ResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);

            bool result = await _category.deleteCategory(id, user); ;
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
