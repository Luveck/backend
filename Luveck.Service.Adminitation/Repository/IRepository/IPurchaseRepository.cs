using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPurchaseRepository
    {
        Task<PurchaseDto> CreatePurchase(PurchaseDto purchaseDto);
        Task<IEnumerable<PurchaseDto>> GetPurchases();
        Task<IEnumerable<PurchaseDto>> GetPurchaseByPharmacy(int idPharmacy);
        Task<IEnumerable<PurchaseDto>> GetPurchaseByClientID(string userName);
        Task<PurchaseDto> GetPurchaseByNoPurchase(string noPurchase);
    }
}
