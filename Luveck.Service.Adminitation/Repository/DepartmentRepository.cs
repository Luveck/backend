using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    
        public async Task<DepartmentDto> GetDepartment(int id)
        {
            Department department = await _appDbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartments()
        {
            List<Department> departmentList = await _appDbContext.Departments.ToListAsync();
            return _mapper.Map<List<DepartmentDto>>(departmentList);
        }
    }
}
