using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPurchaseRepository
    {
        Task<PurshaseResponseDto> CreatePurchase(PurchaseRequestDto purchaseDto, string user);
        Task<PurshaseResponseDto> UpdatePurchase(PurchaseRequestDto purchaseDto, string user);
        Task<List<PurshaseResponseDto>> GetPurchases();
        Task<List<PurshaseResponseDto>> GetPurchaseByPharmacy(int idPharmacy);
        Task<List<PurshaseResponseDto>> GetPurchaseByClientID(string userName);
        Task<PurshaseResponseDto> GetPurchaseByNoPurchase(string noPurchase);
    }
}
