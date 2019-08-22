namespace ReportService.Objects
{
    /// <summary>
    /// Каждая запись справочника объектов и версии данных имеет два поля - ID и Name.
    /// </summary>
    public abstract class БазоваяЗаписьСправочника
    {
        /// <summary>
        /// Уникальный идентификатор объекта справочника.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование объекта справочника.
        /// </summary>
        public string Name { get; set; }
    }
}
