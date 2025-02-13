namespace WebApi.Helpers;

/// <summary>
/// Конфигурация базы данных.
/// </summary>
public class DbSettings
{
    /// <summary>
    /// Сервер.
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// Название базы данных.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }
}