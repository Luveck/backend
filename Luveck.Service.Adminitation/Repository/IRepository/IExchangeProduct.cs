using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IExchangeProduct
    {
        Task<List<ExchangeResponseDto>> ExchangeProduc(List<ExchangeRequestDto> request, string user);
        Task<List<ExchangeProductAvailableResponseDto>> getProductExchangeAvailable (string user);
    }
}
