using Dapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Entities;
using EmployeeAPI.Helpers;
using EmployeeAPI.Interfaces.Repositories;
using System.Data;
using System.Text;

namespace EmployeeAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;

        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> GetIdOrCreateDepartmentAsync(DepartmentDto dto, IDbTransaction? transaction = null)
        {
            var shouldCloseConnection = transaction == null;
            var connection = transaction?.Connection ?? _context.CreateConnection();

            try
            {

                int departmentId;

                var getDepartamentQuery = "" +
                    "SELECT * FROM Departments " +
                    "WHERE Name = @name" +
                    "   AND Phone = @phone";
                var employeeDepartament = await connection.QuerySingleOrDefaultAsync<Employee>(getDepartamentQuery, dto, transaction);

                if (employeeDepartament == null)
                {
                    const string departmentQuery = "INSERT INTO Departments (Name, Phone) " +
                                                   "VALUES(@Name, @Phone) " +
                                                   "RETURNING Id";
                    departmentId = await connection.QuerySingleAsync<int>(departmentQuery, dto);
                }
                else
                    departmentId = employeeDepartament.Id;

                return departmentId;
            }
            finally
            {
                if (shouldCloseConnection)
                    connection.Dispose();
            }
        }

        public async Task<bool> UpdateDepartmentAsync(int departmentId, DepartmentDto dto, IDbTransaction? transaction = null)
        {
            var shouldCloseConnection = transaction == null;
            var connection = transaction?.Connection ?? _context.CreateConnection();

            try
            {

                var updateDepartmentQuery = new StringBuilder("UPDATE Departments SET ");
                var parametersDepartmentQuery = new DynamicParameters();

                if (!string.IsNullOrEmpty(dto.Name))
                {
                    updateDepartmentQuery.Append("Name = @Name, ");
                    parametersDepartmentQuery.Add("Name", dto.Name);
                }

                if (!string.IsNullOrEmpty(dto.Phone))
                {
                    updateDepartmentQuery.Append("Phone = @Phone, ");
                    parametersDepartmentQuery.Add("Phone", dto.Phone);
                }

                updateDepartmentQuery.Length -= 2;
                updateDepartmentQuery.Append(" WHERE Id = @Id");

                parametersDepartmentQuery.Add("Id", departmentId);

                return await connection.ExecuteAsync(updateDepartmentQuery.ToString(), parametersDepartmentQuery, transaction) > 0;
            }
            finally
            {
                if (shouldCloseConnection)
                    connection.Dispose();
            }
        }
    }
}