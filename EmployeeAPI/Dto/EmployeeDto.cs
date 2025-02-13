namespace EmployeeAPI.Dto
{
    /// <summary>
    /// Для передачи данных о сотруднике.
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; init; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public string Phone { get; init; }

        /// <summary>
        /// Идентификатор компании.
        /// </summary>
        public int CompanyId { get; init; }

        /// <summary>
        /// Паспорт.
        /// </summary>
        public PassportDto Passport { get; set; }

        /// <summary>
        /// Департамент.
        /// </summary>
        public DepartmentDto Department { get; set; }
    }
}
