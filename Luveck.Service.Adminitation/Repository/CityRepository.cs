using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using System;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;

namespace Luveck.Service.Administration.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CityResponseDto> CreateCity(CityRequestDto cityDto, string user)
        {
            try
            {
                var cityExist = await _unitOfWork.CityRepository.Find(x => x.Name.ToLower() == cityDto.Name.ToLower());
                var department = await _unitOfWork.DepartmentRepository.Find(x => x.Id == cityDto.departymentId);
                if (department == null) throw new BusinessException(GeneralMessage.DepartmentNoExist);

                if (cityExist != null)
                {
                    throw new BusinessException(GeneralMessage.CityExist);
                }

                City cityNew = new City()
                {
                    Name = cityDto.Name,
                    departmentId = department.Id,
                    CreateBy = user,
                    CreationDate = DateTime.Now,
                    state = true,
                    UpdateBy = user,
                    UpdateDate = DateTime.Now
                };
                await _unitOfWork.CityRepository.InsertAsync(cityNew);

                await _unitOfWork.SaveAsync();

                return await (from city in _unitOfWork.CityRepository.AsQueryable()
                              join dept in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dept.Id
                              join country in _unitOfWork.CountryRepository.AsQueryable() on dept.Country.Id equals country.Id
                              where city.Name == cityDto.Name
                              select new CityResponseDto()
                              {
                                  Id = city.Id,
                                  Name = city.Name,
                                  CreateBy = city.CreateBy,
                                  CreationDate = city.CreationDate,
                                  state = city.state,
                                  UpdateBy = city.UpdateBy,
                                  UpdateDate = city.UpdateDate,
                                  countryId = country.Id,
                                  countryName = country.Name,
                                  departmentName = dept.Name,
                                  departymentId = dept.Id
                              }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<CityResponseDto> UpdateCity(CityRequestDto cityDto, string user)
        {
            try
            {
                var cityExist = await _unitOfWork.CityRepository.Find(x => x.Id == cityDto.Id);
                if (cityExist == null) throw new BusinessException( GeneralMessage.CityNoExist );
                var department = await _unitOfWork.DepartmentRepository.Find(x => x.Id == cityDto.departymentId);
                if (department == null) throw new BusinessException(GeneralMessage.DepartmentNoExist);

                if (!cityExist.Name.ToLower().Equals(cityDto.Name.ToLower()))
                {
                    var cityName = await _unitOfWork.CityRepository.Find(x => x.Name.ToUpper().Equals(cityDto.Name.ToUpper()));
                    if (cityName != null) throw new BusinessException(GeneralMessage.CityExist);
                }
                
                cityExist.state = cityDto.state;
                cityExist.UpdateDate = DateTime.Now;
                cityExist.UpdateBy = user;
                cityExist.department = department;
                cityExist.Name = cityDto.Name;

                _unitOfWork.CityRepository.Update(cityExist);               

                await _unitOfWork.SaveAsync();

                return await (from city in _unitOfWork.CityRepository.AsQueryable()
                                    join dep in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dep.Id
                                    join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                                    where city.Id == cityDto.Id
                                    select new CityResponseDto()
                                    {
                                        Id = city.Id,
                                        Name = city.Name,
                                        CreateBy = city.CreateBy,
                                        state = city.state,
                                        countryId = country.Id,
                                        countryName = country.Name,
                                        CreationDate = city.CreationDate,
                                        UpdateBy = city.UpdateBy,
                                        UpdateDate = city.UpdateDate,
                                        departmentName = dep.Name,
                                        departymentId = dep.Id,
                                    }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteCity(int id, string user)
        {
            try
            {
                var city = await _unitOfWork.CityRepository.Find(x => x.Id == id);
                if (city == null) throw new BusinessException(GeneralMessage.CityNoExist);

                city.UpdateDate = DateTime.Now;
                city.UpdateBy = user;
                city.state = false;

                _unitOfWork.CityRepository.Update(city);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CityResponseDto>> GetCities()
        {
            List<CityResponseDto> lst = await(from city in _unitOfWork.CityRepository.AsQueryable() 
                                              join dep in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dep.Id 
                                              join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                                              select new CityResponseDto()
                                                    {
                                                        Id = city.Id,
                                                        Name = city.Name,
                                                        CreateBy = city.CreateBy,
                                                        state = city.state,
                                                        countryId = country.Id,                       
                                                        countryName = country.Name,
                                                        CreationDate = city.CreationDate,
                                                        UpdateBy = city.UpdateBy,
                                                        UpdateDate = city.UpdateDate,
                                                        departmentName = dep.Name,
                                                        departymentId = dep.Id,

                                                    }).ToListAsync();
            return lst;
        }

        public async Task<List<CityResponseDto>> GetCitiesByDepartment(int departmentId)
        {
            List<CityResponseDto> lst = await (from city in _unitOfWork.CityRepository.AsQueryable()
                                               join dep in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dep.Id
                                               join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                                               where dep.Id == departmentId
                                               select new CityResponseDto()
                                               {
                                                   Id = city.Id,
                                                   Name = city.Name,
                                                   CreateBy = city.CreateBy,
                                                   state = city.state,
                                                   countryId = country.Id,
                                                   countryName = country.Name,
                                                   CreationDate = city.CreationDate,
                                                   UpdateBy = city.UpdateBy,
                                                   UpdateDate = city.UpdateDate,
                                                   departmentName = dep.Name,
                                                   departymentId = dep.Id,

                                               }).ToListAsync();
            return lst;
        }

        public async Task<CityResponseDto> GetCityById(int id)
        {
            return await (from city in _unitOfWork.CityRepository.AsQueryable()
                          join dep in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dep.Id
                          join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                          where city.Id == id
                          select new CityResponseDto()
                          {
                              Id = city.Id,
                              Name = city.Name,
                              CreateBy = city.CreateBy,
                              state = city.state,
                              countryId = country.Id,
                              countryName = country.Name,
                              CreationDate = city.CreationDate,
                              UpdateBy = city.UpdateBy,
                              UpdateDate = city.UpdateDate,
                              departmentName = dep.Name,
                              departymentId = dep.Id,
                          }).FirstOrDefaultAsync();
        }

        public async Task<CityResponseDto> GetCityByName(string name)
        {
            return await (from city in _unitOfWork.CityRepository.AsQueryable()
                          join dep in _unitOfWork.DepartmentRepository.AsQueryable() on city.department.Id equals dep.Id
                          join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                          where city.Name.ToUpper().Equals(name.ToUpper())
                          select new CityResponseDto()
                          {
                              Id = city.Id,
                              Name = city.Name,
                              CreateBy = city.CreateBy,
                              state = city.state,
                              countryId = country.Id,
                              countryName = country.Name,
                              CreationDate = city.CreationDate,
                              UpdateBy = city.UpdateBy,
                              UpdateDate = city.UpdateDate,
                              departmentName = dep.Name,
                              departymentId = dep.Id,
                          }).FirstOrDefaultAsync();
        }
    }
}
