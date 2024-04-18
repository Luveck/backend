using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryDto> CreateCountry(CountryDto countryDto)
        {
            try
            {
                Country country = await _unitOfWork.CountryRepository.Find(x => x.Name.ToLower() == countryDto.Name.ToLower());
                if (country != null) throw new BusinessException(GeneralMessage.CountryExist);

                Country countryNew = new Country()
                {
                    CreateBy = countryDto.CreateBy,
                    CreationDate = countryDto.CreationDate,
                    Currency = countryDto.Currency,
                    CurrencySymbol = countryDto.CurrencySymbol,
                    CurrencyName = countryDto.CurrencyName,
                    Iso3 = countryDto.Iso3,
                    Name = countryDto.Name,
                    PhoneCode = countryDto.PhoneCode,
                    Status = true,
                    UpdateBy = countryDto.UpdateBy,
                    UpdateDate = countryDto.UpdateDate
                };

                await _unitOfWork.CountryRepository.InsertAsync(countryNew);
                await _unitOfWork.SaveAsync();

                countryDto.Id = countryNew.Id;

                return countryDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CountryDto> UpdateCountry(CountryDto countryDto)
        {
            try
            {
                Country country = await _unitOfWork.CountryRepository.Find(x => x.Id == countryDto.Id);
                if (country == null) throw new BusinessException(GeneralMessage.CountryNoExist);

                if(!country.Name.ToLower().Equals(countryDto.Name.ToLower()))
                {
                    Country exist = await _unitOfWork.CountryRepository.Find(x => x.Name.ToLower().Equals(countryDto.Name.ToLower()));
                    if (exist != null) throw new BusinessException(GeneralMessage.CountryExist);
                }

                country.Name = countryDto.Name;
                country.PhoneCode = countryDto.PhoneCode;
                country.CurrencyName = countryDto.CurrencyName;
                country.Currency = countryDto.Currency;
                country.CurrencySymbol = countryDto.CurrencySymbol;
                country.Iso3 = countryDto.Iso3;
                country.Status = countryDto.Status;
                country.UpdateBy = countryDto.UpdateBy;
                country.UpdateDate = countryDto.UpdateDate;

                _unitOfWork.CountryRepository.Update(country);
                await _unitOfWork.SaveAsync();                

                return countryDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteCountry(int Id, string user)
        {
            try
            {
                Country exist = await _unitOfWork.CountryRepository.Find(x => x.Id == Id);   

                if (exist == null) throw new BusinessException(GeneralMessage.CountryNoExist);

                exist.Status = false;
                exist.UpdateDate = DateTime.Now;
                exist.UpdateBy = user;
                _unitOfWork.CountryRepository.Update(exist);
                await _unitOfWork.SaveAsync();
  
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CountryDto>> GetCountries()
        {
            List<CountryDto> countryList = await _unitOfWork.CountryRepository.AsQueryable().Select(x => new CountryDto()
            {
                Id = x.Id,
                CreateBy= x.CreateBy,
                CreationDate= x.CreationDate,
                Currency = x.Currency,
                Name = x.Name,
                PhoneCode = x.PhoneCode,
                Status = x.Status,
                CurrencyName= x.CurrencyName,
                CurrencySymbol= x.CurrencySymbol,
                Iso3 = x.Iso3,
                UpdateBy = x.UpdateBy,
                UpdateDate = x.UpdateDate,
            }).ToListAsync();
            return countryList;
        }

        public async Task<CountryDto> GetCountry(int id)
        {
            var exist = await _unitOfWork.CountryRepository.Find(x => x.Id == id);

            if(exist== null) return null;
            
            return new CountryDto() { 
                Id = id,
                CreateBy = exist.CreateBy,
                CreationDate = exist.CreationDate,
                Currency = exist.Currency,
                CurrencyName = exist.CurrencyName,
                CurrencySymbol = exist.CurrencySymbol,
                Iso3 = exist.Iso3,
                Name = exist.Name,
                PhoneCode = exist.PhoneCode,
                Status = exist.Status,
                UpdateBy = exist.UpdateBy,
                UpdateDate = exist.UpdateDate
            };            
        }

        public async Task<CountryDto> GetCountryName(string name)
        {
            var exist = await _unitOfWork.CountryRepository.Find(x => x.Name.ToLower() == name.ToLower());

            if (exist == null) return null;

            return new CountryDto()
            {
                Id = exist.Id,
                CreateBy = exist.CreateBy,
                CreationDate = exist.CreationDate,
                Currency = exist.Currency,
                CurrencyName = exist.CurrencyName,
                CurrencySymbol = exist.CurrencySymbol,
                Iso3 = exist.Iso3,
                Name = exist.Name,
                PhoneCode = exist.PhoneCode,
                Status = exist.Status,
                UpdateBy = exist.UpdateBy,
                UpdateDate = exist.UpdateDate
            };
        }
    }
}
