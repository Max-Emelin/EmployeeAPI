using EmployeeAPI.Dto;
using EmployeeAPI.Entities;

namespace EmployeeAPI.Interfaces.Repositories
{
    /// <summary>
    /// Для сотрудников.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Асинхронное создание сотрудника по <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"> Сотрудник. </param>
        /// <returns> Идентификатор нового сотрудника. </returns>
        Task<int> CreateEmployeeAsync(EmployeeDto dto);

        /// <summary>
        /// Асинхронное удаление сотрудника по идентификатору <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> Идентификатор сотрудника. </param>
        /// <returns> True, если удаление успешно, иначе false.</returns>
        Task<bool> DeleteEmployeeByIdAsync(int id);

        /// <summary>
        /// Асинхронное получение сотрудников по идентификатору компании <paramref name="companyId"/>.
        /// </summary>
        /// <param name="companyId"> Идентификатор компании. </param>
        /// <returns> Сотрудников, которые принадлежат данной компании. </returns>
        Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyIdAsync(int companyId);

        /// <summary>
        /// Асинхронное получение сотрудников по названию департамента <paramref name="departmentName"/>.
        /// </summary>
        /// <param name="departmentName"> Название департамента. </param>
        /// <returns> Сотрудников, которые находятся в данном департаменте. </returns>
        Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentNameAsync(string departmentName);

        /// <summary>
        /// Асинхронное обновление сотрудника.
        /// </summary>
        /// <param name="dto"> Сотрудник. </param>
        /// <returns> True, если обновление успешно, иначе false.</returns>
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto);

        /// <summary>
        /// Асинхронное получение сотрудника по идентификатору <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> Идентификатор сотрудника. </param>
        /// <returns> Сотрудника. </returns>
        Task<Employee> GetEmployeeByIdAsync(int id);
    }
}