namespace DirectoryService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class МетаданныеСправочника
    {
        public МетаданныеСправочника() { }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="МетаданныеСправочника"/> class.
        /// Предпочтительный конструктор класса МетаданныеСправочника.
        /// </summary>
        /// <param name="элементСправочника">Элемент справочника. Какой элемент передашь - такие метаданные и отдадутся.</param>
        public МетаданныеСправочника(object элементСправочника)
        {
            // Определим по объекту справочника, что это за справочник?
            if (элементСправочника is ОбъектСтроительства)
            {
                this.id_sprav = 1;
                this.name_sprav = "Справочник Объекты строительства";
                this.attr_sprav = this.ПолучитьАтрибутСправочника(typeof(ОбъектСтроительства));
            }
            else if (элементСправочника is ВерсияДанных)
            {
                this.id_sprav = 2;
                this.name_sprav = "Справочник Версии данных";
                this.attr_sprav = this.ПолучитьАтрибутСправочника(typeof(ВерсияДанных));
            }
            else
            {
                throw new NotSupportedException($"Указанный тип справочника не поддерживается.");
            }
        }

        private List<АтрибутСправочника> ПолучитьАтрибутСправочника(System.Type типЭлементаСправочника)
        {
            PropertyInfo[] props = типЭлементаСправочника.GetProperties(); // typeof(ВерсияДанных).GetProperties();

            List<АтрибутСправочника> атрибутыСправочника = new List<АтрибутСправочника>();

            if (props.Any())
            {
                foreach (var prop in props)
                {
                    атрибутыСправочника.Add(new АтрибутСправочника(prop));
                }
            }

            return атрибутыСправочника;
        }
    }
}
