namespace EmployeeAPI.Dto;

/// <summary>
/// Для передачи данных при обновлении сотрудника.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
/// <param name="Name"> Имя. </param>
/// <param name="Surname"> Фамилия. </param>
/// <param name="Phone"> Телефон. </param>
/// <param name="CompanyId"> Идентификатор компании. </param>
/// <param name="Passport"> Паспорт. </param>
/// <param name="Department"> Департамент. </param>
public record UpdateEmployeeDto(
    int Id,
    string? Name,
    string? Surname,
    string? Phone,
    int? CompanyId,
    PassportDto? Passport,
    DepartmentDto? Department
);
