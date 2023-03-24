using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPharmacyRepository
    {
        Task<List<PharmacyResponseDto>> GetPharmacies();
        Task<List<PharmacyResponseDto>> GetPharmaciesByCity(int idCity);
        Task<List<PharmacyResponseDto>> GetPharmaciesByName(string name);
        Task<PharmacyResponseDto> GetPharmacy(int id);
        Task<PharmacyResponseDto> UpdatePharmacy(PharmacyRequestDto pharmacyDto, string user);
        Task<PharmacyResponseDto> CreatePharmacy(PharmacyRequestDto pharmacyDto, string user);
        Task<bool> deletePharmacy(int id, string user);
    }
}
