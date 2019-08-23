namespace ReportService.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Класс для десериализации запроса структуры справочника.
    /// </summary>
    public class МетаданныеСправочника
    {
        /// <summary>
        /// Идентификатор справочника. Для Об. стр. = 1, для версий данных = 2.
        /// Тут лучше использовать Guid, но в рамках тест. задания - int.
        /// </summary>
        public int id_sprav { get; set; }

        /// <summary>
        /// Наименование справочника.
        /// </summary>
        public string name_sprav { get; set; }

        /// <summary>
        /// Перечень и свойства атрибутов.
        /// </summary>
        public List<АтрибутСправочника> attr_sprav { get; set; }
    }
}
