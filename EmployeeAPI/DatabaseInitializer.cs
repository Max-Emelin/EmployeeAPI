using Dapper;
using Microsoft.Data.SqlClient;

namespace EmployeeAPI
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqlConnection(_connectionString); 
            await connection.OpenAsync(); 

            var createEmployeeTable = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100),
                    Surname NVARCHAR(100),
                    Phone NVARCHAR(20),
                    CompanyId INT,                   
                    PassportId INT,                   
                    DepartmentId INT,
                    FOREIGN KEY (PassportId) REFERENCES Passports(Id) ON DELETE CASCADE,
                    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id) ON DELETE CASCADE
                )";

            var createPassportTable = @"
                CREATE TABLE IF NOT EXISTS Passports (
                    Id INT PRIMARY KEY IDENTITY,
                    Type NVARCHAR(50),
                    Number NVARCHAR(50)
                )";

            var createDepartmentTable = @"
                CREATE TABLE IF NOT EXISTS Departments (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100),
                    Phone NVARCHAR(20)
                )";

            await connection.ExecuteAsync(createEmployeeTable);   
            await connection.ExecuteAsync(createPassportTable);   
            await connection.ExecuteAsync(createDepartmentTable); 
        }
    }
}
