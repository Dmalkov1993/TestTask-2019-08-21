using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Objects
{
    /// <summary>
    /// Класс для десериализации настроек отчёта.
    /// </summary>
    public class НастройкаОтчёта
    {
        /// <summary>
        /// Идентификатор справочника по строкам.
        /// </summary>
        public int ID_DirectoryOnRows { get; set; }

        /// <summary>
        /// Идентификатор справочника по столбцам.
        /// </summary>
        public int ID_DirectoryOnColumns { get; set; }

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
