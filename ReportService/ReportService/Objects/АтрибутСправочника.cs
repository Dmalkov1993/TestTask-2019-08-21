using System.Reflection;

namespace ReportService.Objects
{
    /// <summary>
    /// Класс, описывающий атрибуты свойств справочника.
    /// </summary>
    public class АтрибутСправочника
    {
        /// <summary>
        /// Имя атрибута.
        /// </summary>
        public string AttrName { get; set; }

        /// <summary>
        /// Тип атрибута.
        /// </summary>
        public string AttrType { get; set; }
    }
}
