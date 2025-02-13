using EmployeeAPI.Helpers;
using EmployeeAPI.Interfaces.Repositories;
using EmployeeAPI.Interfaces.Services;
using EmployeeAPI.Repositories;
using EmployeeAPI.Services;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddCors();
    services.AddControllers();

    services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

    services.AddSingleton<DataContext>();
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    services.AddScoped<IPassportRepository, PassportRepository>();
    services.AddScoped<IEmployeeService, EmployeeService>();
}

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

{

    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());


    app.MapControllers();
}

app.Run("http://localhost:8080");