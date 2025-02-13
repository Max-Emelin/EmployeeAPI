using Dapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Helpers;
using EmployeeAPI.Interfaces.Repositories;
using System.Text;

namespace EmployeeAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPassportRepository _passportRepository;

        public EmployeeRepository(DataContext context, IDepartmentRepository departmentRepository, IPassportRepository passportRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
            _passportRepository = passportRepository;
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var passportId = await _passportRepository.CreatePassportAsync(dto.Passport, transaction);
            var departmentId = await _departmentRepository.GetIdOrCreateDepartmentAsync(dto.Department, transaction);

            const string employeeQuery = @"INSERT INTO Employees (Name, Surname, Phone, CompanyId, PassportId, DepartmentId)
                                           VALUES(@Name, @Surname, @Phone, @CompanyId, @PassportId, @DepartmentId)
                                           RETURNING Id";
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
            var employeeToDelete = await GetEmployeeByIdAsync(id);

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();


            const string deleteEmployeeQuery = "DELETE " +
                                               "FROM Employees " +
                                               "WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(deleteEmployeeQuery, new { id }, transaction);

            await _passportRepository.DeletePassportByIdAsync(employeeToDelete.PassportId, transaction);

            transaction.Commit();

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

            var employees = await connection.QueryAsync<Employee, Passport, Department, Employee>(
                sqlQuery,
                (employee, passport, department) =>
                {

                    employee.Passport = passport;
                    employee.Department = department;

                    return employee;
                },
                new { CompanyId = companyId },
                splitOn: "PassportType, DepartmentName"
            );

            return employees;
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
            var employeeToUpdate = await GetEmployeeByIdAsync(dto.Id);

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

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
                await _passportRepository.UpdatePassportAsync(employeeToUpdate.PassportId, dto.Passport, transaction);

            if (dto.Department != null)
                await _departmentRepository.UpdateDepartmentAsync(employeeToUpdate.DepartmentId, dto.Department, transaction);

            transaction.Commit();

            return true;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            var getEmployeeToUpdateQuery = "" +
                "SELECT * FROM Employees " +
                "WHERE Id = @id";

            return await connection.QuerySingleOrDefaultAsync<Employee>(getEmployeeToUpdateQuery, new { id });
        }
    }
}
