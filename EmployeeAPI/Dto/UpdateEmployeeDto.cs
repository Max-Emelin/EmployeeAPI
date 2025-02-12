namespace EmployeeAPI.Dto;
public record UpdateEmployeeDto(
    int Id,
    string? Name,
    string? Surname,
    string? Phone,
    int? CompanyId,
    PassportDto? Passport,
    DepartmentDto? Department
);
