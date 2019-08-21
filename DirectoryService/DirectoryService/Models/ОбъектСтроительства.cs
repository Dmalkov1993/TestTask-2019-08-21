namespace DirectoryService.Models
{
    /// <summary>
    /// Класс, описывающий запись в справочнике "ConstructionObjects" (Объекты строительства).
    /// </summary>
    public class ОбъектСтроительства : БазоваяЗаписьСправочника
    {
        /// <summary>
        /// Код объекта строительства.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Запланированный бюджет, млн. руб.
        /// </summary>
        public double Budget { get; set; }
    }
}
