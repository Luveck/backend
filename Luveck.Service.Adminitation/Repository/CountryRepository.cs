﻿using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;

        public CountryRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<CountryDto> CreateUpdateCountry(CountryDto countryDto)
        {
            Country country = _mapper.Map<Country>(countryDto);

            if (country.Id > 0)
            {
                _appDbContext.Country.Update(country);
            }
            else
            {
                _appDbContext.Country.Add(country);
            }

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CountryDto>(country);
        }

        public async Task<bool> DeleteCountry(int Id)
        {
            try
            {
                Country country = await _appDbContext.Country.FirstOrDefaultAsync(c => c.Id == Id);

                if (country == null) return false;

                _appDbContext.Country.Remove(country);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<CountryDto>> GetCountries()
        {
            List<Country> countryList = await _appDbContext.Country.ToListAsync();
            return _mapper.Map<List<CountryDto>>(countryList);
        }

        public async Task<CountryDto> GetCountry(int id)
        {
            Country country = await _appDbContext.Country.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CountryDto>(country);            
        }

        public async Task<CountryDto> GetCountryName(string name)
        {
            Country country = await _appDbContext.Country.FirstOrDefaultAsync(c => c.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
            return _mapper.Map<CountryDto>(country);
        }
    }
}
