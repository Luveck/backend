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
            var dep = _db.Department.FirstOrDefaultAsync(x => x.Id == cityDto.departymentId);
            if (dep == null) return null;
            
            City city = _mapper.Map<City>(cityDto);
            city.department = dep.Result;

            if (city.Id > 0)
            {
                _db.City.Update(city);
            }
            else
            {
                var cityValid = _db.City.FirstOrDefaultAsync(x => x.Name == cityDto.Name);
                if (cityValid != null) return _mapper.Map<CityDto>(cityValid);
                else _db.City.Add(city);
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
                              state = city.state,
                              departmentName = dep.Name,
                              departymentId = dep.Id,
                              countryId = country.Id,
                              countryName = country.Name,
                              CreateBy = country.CreateBy,
                              CreationDate = country.CreationDate,
                              UpdateBy = city.UpdateBy,
                              UpdateDate = city.UpdateDate,

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
                              state = city.state,
                              departmentName = dep.Name,
                              departymentId = dep.Id,
                              countryId = country.Id,
                              countryName = country.Name,
                              CreateBy = country.CreateBy,
                              CreationDate = country.CreationDate,
                              UpdateBy = city.UpdateBy,
                              UpdateDate = city.UpdateDate,

                          })).FirstOrDefaultAsync();
        }
    }
}
