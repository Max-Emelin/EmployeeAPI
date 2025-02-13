namespace EmployeeAPI.Entities
{
    /// <summary>
    /// Департамент.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public string Phone { get; set; }
    }
}