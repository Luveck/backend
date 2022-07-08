using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Luveck.Service.Administration.Repository
{
    public class CityRepository : ICityRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;

        public CityRepository( AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CityDto> CreateUpdateCity(CityDto cityDto)
        {
            City city = _mapper.Map<City>(cityDto);

            if (city.Id > 0)
            {
                _db.City.Update(city);
            }
            else
            {
                _db.City.Add(city);
            }

            await _db.SaveChangesAsync();

            return _mapper.Map<CityDto>(city);
        }

        public async Task<IEnumerable<CityDto>> GetCities()
        {
            return await (from city in _db.City
                          join dep in _db.Department on city.department.Id equals dep.Id
                          join country in _db.Country on dep.Country.Id equals country.Id
                          select (new CityDto
                          {
                              Id = city.Id,
                              Name = city.Name,
                              StateId = dep.Id.ToString(),
                              StateCode = dep.StateCode,
                              stateName = dep.Name,
                              countryId = country.Id.ToString(),
                              countryName = country.Name,
                              CreateBy = country.CreateBy,
                              CreationDate = country.CreationDate,

                          })).ToListAsync();
        }

        public async Task<CityDto> GetCity(int id)
        {
            return await (from city in _db.City
                          join dep in _db.Department on city.department.Id equals dep.Id
                          join country in _db.Country on dep.Country.Id equals country.Id
                          where city.Id == id
                          select (new CityDto
                          {
                              Id = city.Id,
                              Name = city.Name,
                              StateId = dep.Id.ToString(),
                              StateCode = dep.StateCode,
                              stateName = dep.Name,
                              countryId = country.Id.ToString(),
                              countryName = country.Name,
                              CreateBy = country.CreateBy,
                              CreationDate = country.CreationDate,

                          })).FirstOrDefaultAsync();
        }
    }
}
