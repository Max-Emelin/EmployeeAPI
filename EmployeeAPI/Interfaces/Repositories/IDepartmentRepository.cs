using EmployeeAPI.Dto;
using System.Data;

namespace EmployeeAPI.Interfaces.Repositories
{
    /// <summary>
    /// Для департаментов.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Асинхронное получение идентификатора существующего департамента или создание нового по <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"> Департамент. </param>
        /// <param name="transaction"> Транзакция. </param>
        /// <returns> Идентификатор департамента. </returns>
        Task<int> GetIdOrCreateDepartmentAsync(DepartmentDto dto, IDbTransaction? transaction = null);

        /// <summary>
        /// Асинхронное обновление департамента.
        /// </summary>
        /// <param name="departmentId"> Идентификатор департамента. </param>
        /// <param name="dto"> Департамент для обновления. </param>
        /// <param name="transaction"> Транзакция. </param>
        /// <returns> True, если обновление успешно, иначе false.</returns>
        Task<bool> UpdateDepartmentAsync(int departmentId, DepartmentDto dto, IDbTransaction? transaction = null);
    }
}
