namespace DirectoryService.Models
{
    /// <summary>
    /// Класс, описывающий запись в справочнике "DataVersions" (Версии данных).
    /// </summary>
    public class DataVersion : BaseElementOfDirectory
    {
        /// <summary>
        /// Тип версии данных.
        /// </summary>
        public string VersionType { get; set; }
    }
}
