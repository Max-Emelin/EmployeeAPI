namespace EmployeeAPI.Dto;

/// <summary>
/// Для передачи данных паспорта.
/// </summary>
public class PassportDto
{
    /// <summary>
    /// Тип.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Номер.
    /// </summary>
    public string? Number { get; init; }
};