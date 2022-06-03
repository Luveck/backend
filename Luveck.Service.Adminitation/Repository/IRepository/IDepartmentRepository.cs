using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDto>> GetDepartments();
        Task<DepartmentDto> GetDepartment(int id);
    }
}
