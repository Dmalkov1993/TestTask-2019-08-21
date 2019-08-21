namespace DirectoryService.Models
{
    /// <summary>
    /// Класс, описывающий запись в справочнике "DataVersions" (Версии данных).
    /// </summary>
    public class ВерсияДанных : БазоваяЗаписьСправочника
    {
        /// <summary>
        /// Тип версии данных.
        /// </summary>
        public string VersionType { get; set; }
    }
}
