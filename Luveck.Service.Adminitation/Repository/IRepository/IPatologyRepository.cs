using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IPatologyRepository
    {
        Task<IEnumerable<PatologyDto>> GetPatologies();
        Task<PatologyDto> GetPatology(int id);
        Task<PatologyDto> CreateUpdatePatology(PatologyDto patologyDto);
        Task<bool> deletePatology(int id, string user);
    }
}
