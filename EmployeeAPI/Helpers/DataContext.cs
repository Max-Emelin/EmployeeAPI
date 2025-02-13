namespace EmployeeAPI.Helpers;

using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using WebApi.Helpers;

/// <summary>
/// Контекст данных, использующийся для работы с базой данных.
/// </summary>
public class DataContext
{
    /// <summary>
    /// Конфигурация базы данных.
    /// </summary>
    private DbSettings _dbSettings;

    /// <summary>
    /// Для инициализации контекста данных.
    /// </summary>
    /// <param name="dbSettings"> Конфигурация базы данных. </param>
    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    /// <summary>
    /// Создание подключение к базе данных.
    /// </summary>
    /// <returns> Подключение к базе данных. </returns>
    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserName}; Password={_dbSettings.Password};";

        return new NpgsqlConnection(connectionString);
    }

    /// <summary>
    /// Асинхронно инициализирует базу данных и таблицы, если они не существуют.
    /// </summary>
    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    /// <summary>
    /// Асинхронная инициализация базы данных, если она не существует.
    /// </summary>
    private async Task _initDatabase()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserName}; Password={_dbSettings.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    /// <summary>
    /// Асинхронная инициализация таблиц, если они не существуют.
    /// </summary>
    private async Task _initTables()
    {
        using var connection = CreateConnection();
        await _initPassports();
        await _initDepartments();
        await _initEmployees();

        async Task _initPassports()
        {
            var createPassportsTable = @"
                CREATE TABLE IF NOT EXISTS Passports (
                    Id SERIAL PRIMARY KEY,
                    Type VARCHAR(50),
                    Number VARCHAR(50) UNIQUE
                )";

            await connection.ExecuteAsync(createPassportsTable);
        }

        async Task _initDepartments()
        {
            var createDepartmentsTable = @"
                CREATE TABLE IF NOT EXISTS Departments (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(100),
                    Phone VARCHAR(20)
                )";

            await connection.ExecuteAsync(createDepartmentsTable);
        }

        async Task _initEmployees()
        {
            var createEmployeesTable = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(100) NOT NULL,
                    Surname VARCHAR(100) NOT NULL,
                    Phone VARCHAR(20),
                    CompanyId INT,
                    PassportId INT,
                    DepartmentId INT,
                    FOREIGN KEY (PassportId) REFERENCES Passports(Id),
                    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id) 
                )";

            await connection.ExecuteAsync(createEmployeesTable);
        }
    }
}