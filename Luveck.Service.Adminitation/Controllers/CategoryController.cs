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
    [ApiExplorerSettings(GroupName = "ApiAdminCategory")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class CategoryController : ControllerBase
    {
        public ICategoryRepository _category;

        public CategoryController(ICategoryRepository category)
        {
            _category = category;
        }

        [HttpGet]
        [Route("GetCategories")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _category.GetCategories();
            return Ok(categories);
        }

        [HttpGet]
        [Route("GetCategoryById")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatedoryById(int Id)
        {
            var category = await _category.GetCategory(Id);
            return Ok(category);
        }

        [HttpPost]
        [Route("CreateCategory")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory(CategoryDto categoryDto, string user)
        {
            if (string.IsNullOrEmpty(user.Trim())) return BadRequest(new
            {
                message = "El usuario no puede ir vacio"
            });
            categoryDto.CreationDate = DateTime.Now;
            categoryDto.CreateBy = user;
            categoryDto.UpdateDate = DateTime.Now;
            categoryDto.UpdateBy = user;
            categoryDto.IsDeleted = false;
            CategoryDto category = await _category.CreateUpdateCategory(categoryDto);
            return Ok(category);
        }

        [HttpPut]
        [Route("UpdateCategory")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto, string user)
        {
            if (string.IsNullOrEmpty(user.Trim())) return BadRequest(new
            {
                message = "El usuario no puede ir vacio"
            });
            categoryDto.UpdateDate = DateTime.Now;
            categoryDto.UpdateBy = user;
            CategoryDto category = await _category.CreateUpdateCategory(categoryDto);
            return Ok(category);
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var category = await _category.deleteCategory(Id);
            return Ok(category);
        }
    }
}
