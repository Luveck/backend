using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPharmacyRepository
    {
        Task<IEnumerable<PharmacyDto>> GetPharmacies();
        Task<IEnumerable<PharmacyDto>> GetPharmaciesByCity(int idCity);
        Task<IEnumerable<PharmacyDto>> GetPharmaciesByName(string name);
        Task<PharmacyDto> GetPharmacy(int id);
        Task<PharmacyDto> CreateUpdatePharmacy(PharmacyDto pharmacyDto);
        Task<bool> deletePharmacy(int id, string user);
    }
}
