namespace DirectoryService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class DirectoryMetadata
    {
        public DirectoryMetadata() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryMetadata"/> class.
        /// Предпочтительный конструктор класса МетаданныеСправочника.
        /// </summary>
        /// <param name="directoryElement">Элемент справочника. Какой элемент передашь - такие метаданные и отдадутся.</param>
        public DirectoryMetadata(object directoryElement)
        {
            // Определим по объекту справочника, что это за справочник?
            if (directoryElement is ConstructionObject)
            {
                this.DirectoryId = 1;
                this.DirectoryName = "Справочник Объекты строительства";
                this.DirectoryAttributes = this.GetDirectoryAttributes(typeof(ConstructionObject));
            }
            else if (directoryElement is DataVersion)
            {
                this.DirectoryId = 2;
                this.DirectoryName = "Справочник Версии данных";
                this.DirectoryAttributes = this.GetDirectoryAttributes(typeof(DataVersion));
            }
            else
            {
                throw new NotSupportedException($"Указанный тип справочника не поддерживается.");
            }
        }

        /// <summary>
        /// Идентификатор справочника. Для Об. стр. = 1, для версий данных = 2.
        /// Тут лучше использовать Guid, но в рамках тест. задания - int.
        /// </summary>
        public int DirectoryId { get; set; }

        /// <summary>
        /// Наименование справочника.
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        /// Перечень и свойства атрибутов.
        /// </summary>
        public List<DirectoryAttribute> DirectoryAttributes { get; set; }

        private List<DirectoryAttribute> GetDirectoryAttributes(Type typeOfDirectoryElement)
        {
            PropertyInfo[] props = typeOfDirectoryElement.GetProperties(); // typeof(ВерсияДанных).GetProperties();

            List<DirectoryAttribute> directoryAttributes = new List<DirectoryAttribute>();

            if (props.Any())
            {
                foreach (var prop in props)
                {
                    directoryAttributes.Add(new DirectoryAttribute(prop));
                }
            }

            return directoryAttributes;
        }
    }
}
