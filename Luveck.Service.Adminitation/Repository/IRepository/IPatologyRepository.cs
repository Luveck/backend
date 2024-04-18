using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPatologyRepository
    {
        Task<List<PatologyResponseDto>> GetPatologies();
        Task<PatologyResponseDto> GetPatologyById(int id);
        Task<PatologyResponseDto> GetPatologyByName(string name);
        Task<PatologyResponseDto> CreatePatology(PatologyRequestDto patologyDto, string user);
        Task<PatologyResponseDto> UpdatePatology(PatologyRequestDto patologyDto, string user);
        Task<bool> deletePatology(int id, string user);
    }
}
