using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponseDto>> GetCategories();
        Task<CategoryResponseDto> GetCategoryById(int id);
        Task<CategoryResponseDto> GetCategoryByName(string name);
        Task<CategoryResponseDto> CreateCategory(CategoryRequestDto categoryDto, string user);
        Task<CategoryResponseDto> UpdateCategory(CategoryRequestDto categoryDto, string user);
        Task<bool> deleteCategory(int id, string user);
    }
}
