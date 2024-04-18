using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IMedicalRepository
    {
        Task<List<MedicalResponseDto>> GetMedicals();
        Task<List<MedicalResponseDto>> GetMedicalByPatolgy(int idPatology);
        Task<List<MedicalResponseDto>> GetMedicalByName(string name);
        Task<MedicalResponseDto> GetMedical(int id);
        Task<MedicalResponseDto> CreateMedical(MedicalRequestDto medicalDto, string user);
        Task<MedicalResponseDto> UpdateMedical(MedicalRequestDto medicalDto, string user);
        Task<bool> deleteMedical(int id, string user);
    }
}
