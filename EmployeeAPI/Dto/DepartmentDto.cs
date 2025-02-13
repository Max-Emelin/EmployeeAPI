namespace EmployeeAPI.Dto;

/// <summary>
/// Для передачи данных департамента.
/// </summary>
public class DepartmentDto
{
    /// <summary>
    /// Название.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Телефон.
    /// </summary>
    public string? Phone { get; init; }
};