using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                _db.Cities.Update(city);
            }
            else
            {
                _db.Cities.Add(city);
            }

            await _db.SaveChangesAsync();

            return _mapper.Map<CityDto>(city);
        }

        public async Task<IEnumerable<CityDto>> GetCities()
        {
            List<City> cityList = await _db.Cities.ToListAsync();
            return _mapper.Map<List<CityDto>>(cityList);
        }

        public async Task<CityDto> GetCity(int id)
        {
            City city = await _db.Cities.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CityDto>(city);
        }
    }
}
