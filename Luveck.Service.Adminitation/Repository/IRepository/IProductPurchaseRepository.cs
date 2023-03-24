using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IProductPurchaseRepository
    {
        Task<string> CreateUpdateProductPurchase(ProductPurchaseRequestDto request, string user);
        Task<bool> DeleteProductsPurchase(ProductPurchaseRequestDto request, string user);
        Task<List<ProductPruchaseResponseDto>> getProductsByPurchase(int purchaseId);
    }
}
