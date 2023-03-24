using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentResponseDto>> GetDepartments();
        Task<List<DepartmentResponseDto>> GetDepartmentsByCountryId(int idCountry);
        Task<DepartmentResponseDto> GetDepartmentById(int id);
        Task<DepartmentResponseDto> GetDepartmentByName(string name);
        Task<DepartmentResponseDto> CreateDepartment(DepartmentCreateUpdateRequestDto departmentDto, string user);
        Task<DepartmentResponseDto> UpdateDepartment(DepartmentCreateUpdateRequestDto departmentDto, string user);
        Task<bool> DeleteDepartment(int id, string user);
    }
}
