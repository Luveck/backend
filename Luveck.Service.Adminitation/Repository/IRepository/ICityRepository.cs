using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ICityRepository
    {
        Task<IEnumerable<CityDto>> GetCities();
        Task<CityDto> GetCity(int id);
        Task<CityDto> CreateUpdateCity(CityDto cityDto);
    }
}
