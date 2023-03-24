using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ICityRepository
    {
        Task<List<CityResponseDto>> GetCities();
        Task<List<CityResponseDto>> GetCitiesByDepartment(int departmentId);
        Task<CityResponseDto> GetCityById(int id);
        Task<CityResponseDto> GetCityByName(string name);
        Task<CityResponseDto> CreateCity(CityRequestDto cityDto, string user);
        Task<CityResponseDto> UpdateCity(CityRequestDto cityDto, string user);
        Task<bool> DeleteCity(int id, string user);
    }
}
