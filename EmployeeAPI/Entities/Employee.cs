namespace EmployeeAPI.Entities
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Идентификатор компании.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Идентификатор паспорта.
        /// </summary>
        public int PassportId { get; set; }

        /// <summary>
        /// Паспорт.
        /// </summary>
        public Passport Passport { get; set; }

        /// <summary>
        /// Идентификатор департамента.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Департамент.
        /// </summary>
        public Department Department { get; set; }
    }
}