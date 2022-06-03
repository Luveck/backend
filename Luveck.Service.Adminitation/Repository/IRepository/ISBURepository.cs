using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ISBURepository
    {
        Task<IEnumerable<SBUDto>> GetSBUs();
        Task<SBUDto> GetSBU(int id);
        Task<SBUDto> CreateUpdateSBU(SBUDto Sbu);
    }
}
