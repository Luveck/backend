using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IMedicalRepository
    {
        Task<IEnumerable<MedicalDto>> GetMedicals();
        Task<IEnumerable<MedicalDto>> GetMedicalByPatolgy(int idPatology);
        Task<IEnumerable<MedicalDto>> GetMedicalByName(string name);
        Task<MedicalDto> GetMedical(int id);
        Task<MedicalDto> CreateUpdateMedical(MedicalDto medicalDto);
        Task<bool> deleteMedical(int id, string user);
    }
}
