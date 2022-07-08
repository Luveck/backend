using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                              StateCode = d.StateCode,
                              countryId = c.Id,
                              countryCode = c.Iso,
                              countryName = c.Name,
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
                              StateCode = d.StateCode,
                              countryId = c.Id,
                              countryCode = c.Iso,
                              countryName = c.Name,
                          })).ToListAsync();
        }
    }
}
