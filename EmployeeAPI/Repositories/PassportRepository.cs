using Dapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Helpers;
using EmployeeAPI.Interfaces.Repositories;
using System.Data;
using System.Text;

namespace EmployeeAPI.Repositories
{
    /// <summary>
    /// Для паспортов.
    /// </summary>
    public class PassportRepository : IPassportRepository
    {
        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Инициализация репозитория для работы с паспортами.
        /// </summary>
        /// <param name="context"> Контекст данных. </param>
        public PassportRepository(DataContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<int> CreatePassportAsync(PassportDto dto, IDbTransaction? transaction = null)
        {
            var shouldCloseConnection = transaction == null;
            var connection = transaction?.Connection ?? _context.CreateConnection();

            try
            {

                const string passportQuery = "INSERT INTO Passports (Type, Number) " +
                                         "VALUES(@Type, @Number) " +
                                         "RETURNING Id";

                return await connection.QuerySingleAsync<int>(passportQuery, dto, transaction);
            }
            finally
            {
                if (shouldCloseConnection)
                    connection.Dispose();
            }
        }

        /// <inheritdoc />
        public async Task<bool> UpdatePassportAsync(int passportId, PassportDto dto, IDbTransaction? transaction = null)
        {
            var shouldCloseConnection = transaction == null;
            var connection = transaction?.Connection ?? _context.CreateConnection();

            try
            {

                var updatePasportQuery = new StringBuilder("UPDATE Passports SET ");
                var parametersPasportQuery = new DynamicParameters();

                if (!string.IsNullOrEmpty(dto.Type))
                {
                    updatePasportQuery.Append("Type = @Type, ");
                    parametersPasportQuery.Add("Type", dto.Type);
                }

                if (!string.IsNullOrEmpty(dto.Number))
                {
                    updatePasportQuery.Append("Number = @Number, ");
                    parametersPasportQuery.Add("Number", dto.Number);
                }

                updatePasportQuery.Length -= 2;
                updatePasportQuery.Append(" WHERE Id = @Id");
                parametersPasportQuery.Add("Id", passportId);

                return await connection.ExecuteAsync(updatePasportQuery.ToString(), parametersPasportQuery, transaction) > 0;
            }
            finally
            {
                if (shouldCloseConnection)
                    connection.Dispose();
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeletePassportByIdAsync(int id, IDbTransaction? transaction = null)
        {
            var shouldCloseConnection = transaction == null;
            var connection = transaction?.Connection ?? _context.CreateConnection();

            try
            {
                const string deleteEmployeeQuery = "DELETE FROM Passports WHERE Id = @id";
                var affectedRows = await connection.ExecuteAsync(deleteEmployeeQuery, new { id }, transaction);

                return affectedRows > 0;
            }
            finally
            {
                if (shouldCloseConnection)
                    connection.Dispose();
            }
        }
    }
}