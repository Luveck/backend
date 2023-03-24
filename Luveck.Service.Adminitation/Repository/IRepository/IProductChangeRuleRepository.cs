using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IProductChangeRuleRepository
    {
        Task<List<ProductRuleChangeResponseDto>> GetRules();
        Task<ProductRuleChangeResponseDto> AddRule(ProductChangeRuleRequestDto request, string userId);
        Task<ProductRuleChangeResponseDto> UpdateRule(ProductChangeRuleRequestDto requestDto, string userId);
        Task<ProductRuleChangeResponseDto> DeleteRule(int id, string userId);
        Task<List<ProductsLandingPageResponseDto>> getProducts();
        Task<ProductRuleChangeResponseDto> GetRuleById(int id);
    }
}
