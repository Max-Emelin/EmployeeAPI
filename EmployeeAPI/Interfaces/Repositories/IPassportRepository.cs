using EmployeeAPI.Dto;
using System.Data;

namespace EmployeeAPI.Interfaces.Repositories
{
    /// <summary>
    /// Для паспортов.
    /// </summary>
    public interface IPassportRepository
    {
        /// <summary>
        /// Асинхронное создание паспорта по <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"> Паспорт. </param>
        /// <param name="transaction"> Транзакция. </param>
        /// <returns></returns>
        Task<int> CreatePassportAsync(PassportDto dto, IDbTransaction? transaction = null);

        /// <summary>
        /// Асинхронное обновление паспорта.
        /// </summary>
        /// <param name="passportId"> Идентификатор паспорта. </param>
        /// <param name="dto"> Паспорт для обновления. </param>
        /// <param name="transaction"> Транзакция. </param>
        /// <returns> True, если обновление успешно, иначе false.</returns>
        Task<bool> UpdatePassportAsync(int passportId, PassportDto dto, IDbTransaction? transaction = null);

        /// <summary>
        /// Асинхронное удаление паспорта по идентификатору <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> Идентификатор паспорта. </param>
        /// <param name="transaction"> Транзакция. </param>
        /// <returns> True, если удаление успешно, иначе false.</returns>
        Task<bool> DeletePassportByIdAsync(int id, IDbTransaction? transaction = null);
    }
}
