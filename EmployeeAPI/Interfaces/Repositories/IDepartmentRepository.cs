using EmployeeAPI.Dto;
using System.Data;

namespace EmployeeAPI.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<int> GetIdOrCreateDepartmentAsync(DepartmentDto dto, IDbTransaction? transaction = null);
        Task<bool> UpdateDepartmentAsync(int departmentId, DepartmentDto dto, IDbTransaction? transaction = null);
    }
}
