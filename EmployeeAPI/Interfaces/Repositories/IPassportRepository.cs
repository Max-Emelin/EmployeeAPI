using EmployeeAPI.Dto;
using System.Data;

namespace EmployeeAPI.Interfaces.Repositories
{
    public interface IPassportRepository
    {
        Task<int> CreatePassportAsync(PassportDto dto, IDbTransaction? transaction = null);
        Task<bool> UpdatePassportAsync(int passportId, PassportDto dto, IDbTransaction? transaction = null);
        Task<bool> DeletePassportByIdAsync(int id, IDbTransaction? transaction = null);
    }
}
