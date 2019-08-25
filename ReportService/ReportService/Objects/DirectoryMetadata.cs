namespace ReportService.Objects
{
    public class DirectoryMetadata
    {
        /// <summary>
        /// Идентификатор справочника. Для Об. стр. = 1, для версий данных = 2.
        /// Тут лучше использовать Guid, но в рамках тест. задания - int.
        /// </summary>
        public int DirectoryId { get; set; }

        /// <summary>
        /// Наименование справочника.
        /// </summary>
        public string DirectoryName { get; set; }
    }
}
