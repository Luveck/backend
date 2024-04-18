using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ICountryRepository 
    {
        Task<List<CountryDto>> GetCountries();
        Task<CountryDto> GetCountry(int id);
        Task<CountryDto> CreateCountry(CountryDto countryDto);
        Task<CountryDto> UpdateCountry(CountryDto countryDto);
        Task<bool> DeleteCountry(int Id, string user);
        Task<CountryDto> GetCountryName(string name);
    }
}
