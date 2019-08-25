namespace ReportService.Objects
{
    using System.Collections.Generic;

    /// <summary>
    /// Класс для десериализации настроек отчёта.
    /// </summary>
    public class ReportSetting
    {
        /// <summary>
        /// Идентификатор настройки.
        /// </summary>
        public int ConfigurationID { get; set; }

        /// <summary>
        /// Идентификатор справочника по строкам.
        /// </summary>
        public int DirectoryIdOnRows { get; set; }

        /// <summary>
        /// Идентификатор справочника по столбцам.
        /// </summary>
        public int DirectoryIdOnColumns { get; set; }

        /// <summary>
        /// Перечень атрибутов справочника по строкам.
        /// </summary>
        public List<string> AttributesOfDirectoryOnRows { get; set; } = new List<string>();

        /// <summary>
        /// Перечень атрибутов справочника по столбцам.
        /// </summary>
        public List<string> AttributesOfDirectoryOnColumns { get; set; } = new List<string>();
    }
}
