namespace EmployeeAPI.Dto;

public record CreateEmployeeDto(
    string Name,
    string Surname,
    string Phone,
    int CompanyId,
    PassportDto Passport,
    DepartmentDto Department
);
