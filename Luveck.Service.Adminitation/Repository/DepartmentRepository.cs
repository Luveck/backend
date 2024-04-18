using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Luveck.Service.Administration.Models;
using System;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using System.Xml.Linq;

namespace Luveck.Service.Administration
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentResponseDto> CreateDepartment(DepartmentCreateUpdateRequestDto departmentDto, string user)
        {
            try
            {
                var department = await _unitOfWork.DepartmentRepository.Find(x => x.Name.ToLower() == departmentDto.Name.ToLower());
                if (department != null) throw new BusinessException(GeneralMessage.DepartmentExist);
                var country = await _unitOfWork.CountryRepository.Find(x => x.Id == departmentDto.idCountry);
                if (country == null) throw new BusinessException(GeneralMessage.CountryNoExist);

                Department deptNew = new Department()
                {
                    Name = departmentDto.Name,                    
                    countryId = country.Id,
                    CreateBy = user,
                    CreationDate = DateTime.Now,
                    status = true,
                    UpdateBy = user,
                    UpdateDate = DateTime.Now
                };

                await _unitOfWork.DepartmentRepository.InsertAsync(deptNew);
                await _unitOfWork.SaveAsync();

                department = await _unitOfWork.DepartmentRepository.Find(x => x.Name.ToLower() == departmentDto.Name.ToLower());

                return await (from dept in _unitOfWork.DepartmentRepository.AsQueryable()
                              join countryR in _unitOfWork.CountryRepository.AsQueryable() on dept.Country.Id equals country.Id
                              where dept.Name.ToUpper().Equals(departmentDto.Name.ToUpper())
                              select new DepartmentResponseDto()
                              {
                                  Id = dept.Id,
                                  Name = dept.Name,
                                  status = dept.status,
                                  countryId = countryR.Id,
                                  countryCode = countryR.Iso3,
                                  countryName = countryR.Name,
                                  CreationDate = dept.CreationDate,
                                  CreateBy = dept.CreateBy,
                                  UpdateBy = dept.UpdateBy,
                                  UpdateDate = dept.UpdateDate,
                              }
                              ).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DepartmentResponseDto> UpdateDepartment(DepartmentCreateUpdateRequestDto departmentDto, string user)
        {
            try
            {
                var department = await _unitOfWork.DepartmentRepository.Find(x => x.Id == departmentDto.Id);
                if (department == null) throw new BusinessException(GeneralMessage.DepartmentNoExist);               
                var country = await _unitOfWork.CountryRepository.Find(x => x.Id == departmentDto.idCountry);
                if (country == null) throw new BusinessException(GeneralMessage.CountryNoExist);

                if(department.Name != departmentDto.Name)
                {
                    var departmentName = await _unitOfWork.DepartmentRepository.Find(x => x.Name.ToLower() == departmentDto.Name.ToLower());
                    if (departmentName != null) throw new BusinessException(GeneralMessage.DepartmentExist);
                }

                department.status = departmentDto.status;
                department.UpdateDate = DateTime.Now;
                department.UpdateBy = user;
                department.countryId = country.Id;
                department.Name = departmentDto.Name;

                _unitOfWork.DepartmentRepository.Update(department);
                await _unitOfWork.SaveAsync();

                return await (from dept in _unitOfWork.DepartmentRepository.AsQueryable()
                              join countryR in _unitOfWork.CountryRepository.AsQueryable() on dept.Country.Id equals country.Id
                              where dept.Id == departmentDto.Id
                              select new DepartmentResponseDto()
                              {
                                  Id = dept.Id,
                                  Name = dept.Name,
                                  status = dept.status,
                                  countryId = countryR.Id,
                                  countryCode = countryR.Iso3,
                                  countryName = countryR.Name,
                                  CreationDate = dept.CreationDate,
                                  CreateBy = dept.CreateBy,
                                  UpdateBy = dept.UpdateBy,
                                  UpdateDate = dept.UpdateDate,
                              }
                                              ).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> DeleteDepartment(int id, string user)
        {
            try
            {
                var department = await _unitOfWork.DepartmentRepository.Find(x => x.Id == id);
                if (department == null) throw new BusinessException(GeneralMessage.DepartmentNoExist);

                department.UpdateDate = DateTime.Now;
                department.UpdateBy = user;

                _unitOfWork.DepartmentRepository.Update(department);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DepartmentResponseDto> GetDepartmentById(int id)
        {
            var Dept = await (from dept in _unitOfWork.DepartmentRepository.AsQueryable()
                              join country in _unitOfWork.CountryRepository.AsQueryable() on dept.Country.Id equals country.Id
                              where dept.Id == id
                              select new DepartmentResponseDto()
                              {
                                  Id = dept.Id,
                                  Name = dept.Name,
                                  status = dept.status,
                                  countryId = country.Id,
                                  countryCode = country.Iso3,
                                  countryName = country.Name,
                                  CreationDate = dept.CreationDate,
                                  CreateBy = dept.CreateBy,
                                  UpdateBy = dept.UpdateBy,
                                  UpdateDate = dept.UpdateDate,
                              }
                              ).FirstOrDefaultAsync();

            return Dept;
        }

        public async Task<DepartmentResponseDto> GetDepartmentByName(string name)
        {
            var Dept = await (from dept in _unitOfWork.DepartmentRepository.AsQueryable()
                              join country in _unitOfWork.CountryRepository.AsQueryable() on dept.Country.Id equals country.Id
                              where dept.Name.ToUpper() == name.ToUpper()
                              select new DepartmentResponseDto()
                              {
                                  Id = dept.Id,
                                  Name = dept.Name,
                                  status = dept.status,
                                  countryId = country.Id,
                                  countryCode = country.Iso3,
                                  countryName = country.Name,
                                  CreationDate = dept.CreationDate,
                                  CreateBy = dept.CreateBy,
                                  UpdateBy = dept.UpdateBy,
                                  UpdateDate = dept.UpdateDate,
                              }
                              ).FirstOrDefaultAsync();

            return Dept;
        }

        public async Task<List<DepartmentResponseDto>> GetDepartments()
        {
            List<DepartmentResponseDto> lst = await (from dep in _unitOfWork.DepartmentRepository.AsQueryable()
                                                     join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                                                     select new DepartmentResponseDto()
                                                     {
                                                         Id = dep.Id,
                                                         Name = dep.Name,
                                                         CreateBy = dep.CreateBy,
                                                         status = dep.status, 
                                                         countryId = country.Id,
                                                         countryCode = country.Iso3,
                                                         countryName = country.Name,
                                                         CreationDate = dep.CreationDate,
                                                         UpdateBy = dep.UpdateBy,
                                                         UpdateDate = dep.UpdateDate,

                                                     }).ToListAsync();
            return lst;
        }

        public async Task<List<DepartmentResponseDto>> GetDepartmentsByCountryId(int idCountry)
        {
            var exist = await _unitOfWork.CountryRepository.Find(x => x.Id == idCountry);
            if (exist == null) throw new BusinessException(GeneralMessage.CountryNoExist);

            List<DepartmentResponseDto> lst = await(from dep in _unitOfWork.DepartmentRepository.AsQueryable()
                                                    join country in _unitOfWork.CountryRepository.AsQueryable() on dep.Country.Id equals country.Id
                                                    where country.Id == idCountry
                                                    select new DepartmentResponseDto()
                                                    {
                                                        Id = dep.Id,
                                                        Name = dep.Name,
                                                        CreateBy = dep.CreateBy,
                                                        status = dep.status,
                                                        countryId = country.Id,
                                                        countryCode = country.Iso3,
                                                        countryName = country.Name,
                                                        CreationDate = dep.CreationDate,
                                                        UpdateBy = dep.UpdateBy,
                                                        UpdateDate = dep.UpdateDate,

                                                    }).ToListAsync();
            return lst;
        }
    }
}
