namespace EmployeeAPI.Entities
{
    /// <summary>
    /// Паспорт.
    /// </summary>
    public class Passport
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>
        public string Number { get; set; }
    }
}