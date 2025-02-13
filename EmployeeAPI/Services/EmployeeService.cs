using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Interfaces.Repositories;
using EmployeeAPI.Interfaces.Services;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return await _employeeRepository.CreateEmployeeAsync(dto);
        }

        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            ValidateIdFields(id);

            return await _employeeRepository.DeleteEmployeeByIdAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            ValidateIdFields(companyId);

            return await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            ValidateIdFields(departmentId);

            return await _employeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            ValidateIdFields(dto.Id);

            return await _employeeRepository.UpdateEmployeeAsync(dto);
        }

        private void ValidateIdFields(params int[] ids)
        {
            foreach (var id in ids)
                if (id <= 0)
                    throw new ArgumentException("Invalid id param.", nameof(id));
        }
    }
}
