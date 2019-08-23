using System.Reflection;

namespace DirectoryService.Models
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

        public АтрибутСправочника() { }

        public АтрибутСправочника(string attrName, string attrType)
        {
            this.AttrName = attrName;
            this.AttrType = attrType;
        }

        public АтрибутСправочника(PropertyInfo propertyInfo)
            : this(propertyInfo.Name, propertyInfo.PropertyType.Name)
        { }
    }
}
