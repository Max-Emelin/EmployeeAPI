using EmployeeAPI.Dto;
using EmployeeAPI.Entities;

namespace EmployeeAPI.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<int> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<bool> DeleteEmployeeByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto);
    }
}