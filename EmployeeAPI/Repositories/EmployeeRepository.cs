using Dapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EmployeeAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string passportQuery = "INSERT INTO Passports (Type, Number) " +
                                             "VALUES(@Type, @Number); " +
                                             "SELECT CAST(SCOPE_IDENTITY() AS int)";
                var passportId = await connection.QuerySingleAsync<int>(passportQuery, dto.Passport, transaction);

                const string departmentQuery = "INSERT INTO Departments (Name, Phone) " +
                                               "VALUES(@Name, @Phone); " +
                                               "SELECT CAST(SCOPE_IDENTITY() AS int)";
                var departmentId = await connection.QuerySingleAsync<int>(departmentQuery, dto.Department, transaction);

                const string employeeQuery = @"INSERT INTO Employees (Name, Surname, Phone, CompanyId, PassportId, DepartmentId)
                                               VALUES(@Name, @Surname, @Phone, @CompanyId, @PassportId, @DepartmentId);
                                               SELECT CAST(SCOPE_IDENTITY() AS int)";
                var employeeId = await connection.QuerySingleAsync<int>(employeeQuery, new
                {
                    dto.Name,
                    dto.Surname,
                    dto.Phone,
                    dto.CompanyId,
                    PassportId = passportId,
                    DepartmentId = departmentId
                }, transaction);

                await transaction.CommitAsync();

                return employeeId;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string deleteEmployeeQuery = "DELETE " +
                                                   "FROM Employees " +
                                                   "WHERE Id = @Id";
                var affectedRows = await connection.ExecuteAsync(deleteEmployeeQuery, new { Id = id }, transaction);

                await transaction.CommitAsync();

                return affectedRows > 0;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                const string sqlQuery = @"
                    SELECT e.Id, e.Name, e.Surname, e.Phone, e.CompanyId, 
                        e.PassportId, p.Type AS PassportType, p.Number AS PassportNumber, 
                        e.DepartmentId, d.Name AS DepartmentName, d.Phone AS DepartmentPhone
                    FROM Employees e
                    LEFT JOIN Passports p ON e.PassportId = p.Id
                    LEFT JOIN Departments d ON e.DepartmentId = d.Id
                    WHERE e.CompanyId = @CompanyId";

                var employees = await connection.QueryAsync<Employee>(sqlQuery, new { CompanyId = companyId });

                return employees;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                const string sqlQuery = @"
                    SELECT e.Id, e.Name, e.Surname, e.Phone, e.CompanyId, 
                        e.PassportId, p.Type AS PassportType, p.Number AS PassportNumber, 
                        e.DepartmentId, d.Name AS DepartmentName, d.Phone AS DepartmentPhone
                    FROM Employees e
                    LEFT JOIN Passports p ON e.PassportId = p.Id
                    LEFT JOIN Departments d ON e.DepartmentId = d.Id
                    WHERE e.DepartmentId = @DepartmentId";

                var employees = await connection.QueryAsync<Employee>(sqlQuery, new { DepartmentId = departmentId });

                return employees;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var updateQuery = new StringBuilder("UPDATE Employees SET ");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                updateQuery.Append("Name = @Name, ");
                parameters.Add("Name", dto.Name);
            }

            if (!string.IsNullOrEmpty(dto.Surname))
            {
                updateQuery.Append("Surname = @Surname, ");
                parameters.Add("Surname", dto.Surname);
            }

            if (!string.IsNullOrEmpty(dto.Phone))
            {
                updateQuery.Append("Phone = @Phone, ");
                parameters.Add("Phone", dto.Phone);
            }

            if (dto.CompanyId.HasValue)
            {
                updateQuery.Append("CompanyId = @CompanyId, ");
                parameters.Add("CompanyId", dto.CompanyId);
            }

            if (dto.Passport != null)
            {
                if (!string.IsNullOrEmpty(dto.Passport.Type))
                {
                    updateQuery.Append("PassportType = @PassportType, ");
                    parameters.Add("PassportType", dto.Passport.Type);
                }

                if (!string.IsNullOrEmpty(dto.Passport.Number))
                {
                    updateQuery.Append("PassportNumber = @PassportNumber, ");
                    parameters.Add("PassportNumber", dto.Passport.Number);
                }
            }

            if (dto.Department != null)
            {
                if (!string.IsNullOrEmpty(dto.Department.Name))
                {
                    updateQuery.Append("DepartmentName = @DepartmentName, ");
                    parameters.Add("DepartmentName", dto.Department.Name);
                }

                if (!string.IsNullOrEmpty(dto.Department.Phone))
                {
                    updateQuery.Append("DepartmentPhone = @DepartmentPhone, ");
                    parameters.Add("DepartmentPhone", dto.Department.Phone);
                }
            }

            updateQuery.Length -= 2;

            updateQuery.Append(" WHERE Id = @Id");
            parameters.Add("Id", dto.Id);

            var affectedRows = await connection.ExecuteAsync(updateQuery.ToString(), parameters);

            return affectedRows > 0;
        }
    }
}
