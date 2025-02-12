using Dapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Helpers;
using EmployeeAPI.Interfaces;
using System.Text;

namespace EmployeeAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

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

            transaction.Commit();

            return employeeId;
        }

        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            const string deleteEmployeeQuery = "DELETE " +
                                               "FROM Employees " +
                                               "WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(deleteEmployeeQuery, new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            using var connection = _context.CreateConnection();

            const string sqlQuery = @"
                SELECT e.Id, e.Name, e.Surname, e.Phone, e.CompanyId, 
                    e.PassportId, p.Type AS PassportType, p.Number AS PassportNumber, 
                    e.DepartmentId, d.Name AS DepartmentName, d.Phone AS DepartmentPhone
                FROM Employees e
                LEFT JOIN Passports p ON e.PassportId = p.Id
                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                WHERE e.CompanyId = @CompanyId";

            return await connection.QueryAsync<Employee>(sqlQuery, new { CompanyId = companyId });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId)
        {
            using var connection = _context.CreateConnection();

            const string sqlQuery = @"
                SELECT e.Id, e.Name, e.Surname, e.Phone, e.CompanyId, 
                    e.PassportId, p.Type AS PassportType, p.Number AS PassportNumber, 
                    e.DepartmentId, d.Name AS DepartmentName, d.Phone AS DepartmentPhone
                FROM Employees e
                LEFT JOIN Passports p ON e.PassportId = p.Id
                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                WHERE e.DepartmentId = @DepartmentId";

            return await connection.QueryAsync<Employee>(sqlQuery, new { DepartmentId = departmentId });
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

            var getEmployeeToUpdateQuery = "" +
                "SELECT * FROM Users " +
                "WHERE Id = @id";
            var employeeToUpdate = await connection.QuerySingleOrDefaultAsync<Employee>(getEmployeeToUpdateQuery, new { dto.Id }, transaction);

            var updateEmployeeQuery = new StringBuilder("UPDATE Employees SET ");
            var parametersEmployeeQuery = new DynamicParameters();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                updateEmployeeQuery.Append("Name = @Name, ");
                parametersEmployeeQuery.Add("Name", dto.Name);
            }

            if (!string.IsNullOrEmpty(dto.Surname))
            {
                updateEmployeeQuery.Append("Surname = @Surname, ");
                parametersEmployeeQuery.Add("Surname", dto.Surname);
            }

            if (!string.IsNullOrEmpty(dto.Phone))
            {
                updateEmployeeQuery.Append("Phone = @Phone, ");
                parametersEmployeeQuery.Add("Phone", dto.Phone);
            }

            if (dto.CompanyId.HasValue)
            {
                updateEmployeeQuery.Append("CompanyId = @CompanyId, ");
                parametersEmployeeQuery.Add("CompanyId", dto.CompanyId);
            }

            updateEmployeeQuery.Length -= 2;
            updateEmployeeQuery.Append(" WHERE Id = @Id");
            parametersEmployeeQuery.Add("Id", dto.Id);
            await connection.ExecuteAsync(updateEmployeeQuery.ToString(), parametersEmployeeQuery, transaction);

            if (dto.Passport != null)
            {
                var updatePasportQuery = new StringBuilder("UPDATE Employees SET ");
                var parametersPasportQuery = new DynamicParameters();

                if (!string.IsNullOrEmpty(dto.Passport.Type))
                {
                    updatePasportQuery.Append("Type = @Type, ");
                    parametersPasportQuery.Add("Type", dto.Passport.Type);
                }

                if (!string.IsNullOrEmpty(dto.Passport.Number))
                {
                    updatePasportQuery.Append("Number = @Number, ");
                    parametersPasportQuery.Add("Number", dto.Passport.Number);
                }

                updatePasportQuery.Length -= 2;
                updatePasportQuery.Append(" WHERE Id = @Id");
                parametersPasportQuery.Add("Id", employeeToUpdate.PassportId);
                await connection.ExecuteAsync(updatePasportQuery.ToString(), parametersPasportQuery, transaction);
            }

            if (dto.Department != null)
            {
                var updateDepartmentQuery = new StringBuilder("UPDATE Departments SET ");
                var parametersDepartmentQuery = new DynamicParameters();

                if (!string.IsNullOrEmpty(dto.Department.Name))
                {
                    updateDepartmentQuery.Append("DepartmentName = @DepartmentName, ");
                    parametersDepartmentQuery.Add("DepartmentName", dto.Department.Name);
                }

                if (!string.IsNullOrEmpty(dto.Department.Phone))
                {
                    updateDepartmentQuery.Append("DepartmentPhone = @DepartmentPhone, ");
                    parametersDepartmentQuery.Add("DepartmentPhone", dto.Department.Phone);
                }

                updateDepartmentQuery.Length -= 2;
                updateDepartmentQuery.Append(" WHERE Id = @Id");

                parametersDepartmentQuery.Add("Id", employeeToUpdate.DepartmentId);

                await connection.ExecuteAsync(updateDepartmentQuery.ToString(), parametersDepartmentQuery, transaction);
            }
            transaction.Commit();

            return true;
        }
    }
}
