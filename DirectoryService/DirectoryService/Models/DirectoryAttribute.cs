using System.Reflection;

namespace DirectoryService.Models
{
    /// <summary>
    /// Класс, описывающий атрибуты свойств справочника.
    /// </summary>
    public class DirectoryAttribute
    {
        /// <summary>
        /// Имя атрибута.
        /// </summary>
        public string AttrName { get; set; }

        /// <summary>
        /// Тип атрибута.
        /// </summary>
        public string AttrType { get; set; }

        public DirectoryAttribute() { }

        public DirectoryAttribute(string attrName, string attrType)
        {
            this.AttrName = attrName;
            this.AttrType = attrType;
        }

        public DirectoryAttribute(PropertyInfo propertyInfo)
            : this(propertyInfo.Name, propertyInfo.PropertyType.Name)
        { }
    }
}
