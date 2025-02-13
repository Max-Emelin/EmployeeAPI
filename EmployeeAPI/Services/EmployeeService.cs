using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Interfaces.Repositories;
using EmployeeAPI.Interfaces.Services;

namespace EmployeeAPI.Services
{
    /// <inheritdoc />
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// Репозиторий для работы с сотрудниками.
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Для инициализации сервиса сотрудников.
        /// </summary>
        /// <param name="employeeRepository"> Репозиторий для работы с сотрудниками. </param>
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <inheritdoc />
        public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return await _employeeRepository.CreateEmployeeAsync(dto);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            ValidateIdFields(id);

            return await _employeeRepository.DeleteEmployeeByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            ValidateIdFields(companyId);

            return await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
            return await _employeeRepository.GetEmployeesByDepartmentNameAsync(departmentName);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            ValidateIdFields(dto.Id);

            return await _employeeRepository.UpdateEmployeeAsync(dto);
        }

        /// <inheritdoc />
        private void ValidateIdFields(params int[] ids)
        {
            foreach (var id in ids)
                if (id <= 0)
                    throw new ArgumentException("Invalid id param.", nameof(id));
        }
    }
}