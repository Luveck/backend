using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Luveck.Service.Administration.Models;
using System;

namespace Luveck.Service.Administration
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;

        public DepartmentRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<DepartmentsDto> CreateUpdateDepartment(DepartmentsDto departmentDto)
        {
            Department department = _mapper.Map<Department>(departmentDto);
            var country = await _appDbContext.Country.FirstOrDefaultAsync(c => c.Id == departmentDto.countryId);
            if (country == null) return null;

            department.Country = country;
            department.UpdateDate = DateTime.Now;
            department.UpdateBy = departmentDto.UpdateBy;

            if (department.Id > 0)
            {      
                _appDbContext.Department.Update(department);
            }
            else
            {
                department.CreationDate = DateTime.Now;
                department.CreateBy = departmentDto.UpdateBy;
                _appDbContext.Department.Add(department);
            }

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<DepartmentsDto>(department);
        }

        public async Task<DepartmentsDto> GetDepartment(int id)
        {
            return await (from d in _appDbContext.Department
                          join c in _appDbContext.Country on d.Country.Id equals c.Id
                          where c.Id == id
                          select (
                          new DepartmentsDto
                          {
                              Id = d.Id,
                              Name = d.Name,
                              status = d.status,
                              countryId = c.Id,
                              countryCode = c.Iso3,
                              countryName = c.Name,
                              CreationDate = c.CreationDate,
                              UpdateBy = c.UpdateBy,
                              UpdateDate = c.UpdateDate,
                          })).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DepartmentsDto>> GetDepartments()
        {
            return await (from d in _appDbContext.Department
                          join c in _appDbContext.Country on d.Country.Id equals c.Id
                          select (
                          new DepartmentsDto { 
                              Id= d.Id, 
                              Name = d.Name, 
                              status = d.status,
                              countryId = c.Id,
                              countryCode = c.Iso3,
                              countryName = c.Name,
                              CreationDate = c.CreationDate,
                              UpdateBy = c.UpdateBy,
                              UpdateDate = c.UpdateDate,
                          })).ToListAsync();
        }
    }
}
