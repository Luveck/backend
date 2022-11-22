using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ICountryRepository 
    {
        Task<IEnumerable<CountryDto>> GetCountries();
        Task<CountryDto> GetCountry(int id);
        Task<CountryDto> CreateUpdateCountry(CountryDto countryDto);
        Task<bool> DeleteCountry(int Id);
        Task<CountryDto> GetCountryName(string name);
    }
}
