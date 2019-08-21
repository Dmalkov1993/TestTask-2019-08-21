namespace DirectoryService.DataLoaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using DirectoryService.Models;

    /// <summary>
    /// Класс по получению данных из файлов справочников.
    /// </summary>
    public static class DataLoader
    {
        /// <summary>
        /// Метод по получению списка объектов справочника "ОбъектыСтроительства".
        /// </summary>
        /// <returns>Список объектов справочника "ОбъектыСтроительства".</returns>
        public static Dictionary<int, ОбъектСтроительства> ПолучитьДанныеСправочникаОбъектыСтроительства()
        {
            // Прочитаем Excel файл с данными:
            Dictionary<int, ОбъектСтроительства> объектыСтроительства = new Dictionary<int, ОбъектСтроительства>();

            // Открываем файл
            XLWorkbook workBook = new XLWorkbook("DataSources\\ConstructionObjects.xlsx");
            IXLWorksheet workSheet = workBook.Worksheet(1);

            int indexOfLastRow = workSheet.RangeUsed().LastRowUsed().RangeAddress.FirstAddress.RowNumber;

            // Читаем со второй строки, так как первая это шапка-заголовок
            var wshRows = workSheet.Rows(2, indexOfLastRow);

            foreach (IXLRow row in wshRows)
            {
                IXLCells wshCells = row.Cells("1:4");

                int id = Convert.ToInt32(row.Cell(1).Value);

                // Создаём и добавляем элементы в List.
                объектыСтроительства.Add(
                    id,
                    new ОбъектСтроительства
                    {
                        ID = id,
                        Name = row.Cell(2).Value.ToString(),
                        Code = row.Cell(3).Value.ToString(),
                        Budget = (double)row.Cell(4).Value,
                    });
            }

            return объектыСтроительства;
        }

        /// <summary>
        /// Метод по получению списка объектов справочника "ОбъектыСтроительства".
        /// </summary>
        /// <returns>Список объектов справочника "ОбъектыСтроительства".</returns>
        public static Dictionary<int, ВерсияДанных> ПолучитьДанныеСправочникаВерсииДанных()
        {
            // Прочитаем Excel файл с данными:
            Dictionary<int, ВерсияДанных> версииДанных = new Dictionary<int, ВерсияДанных>();

            // Открываем файл
            XLWorkbook workBook = new XLWorkbook("DataVersions.xlsx");
            IXLWorksheet workSheet = workBook.Worksheet(1);

            int indexOfLastRow = workSheet.RangeUsed().LastRowUsed().RangeAddress.FirstAddress.RowNumber;

            // Читаем со второй строки, так как первая это шапка-заголовок
            var wshRows = workSheet.Rows(2, indexOfLastRow);

            foreach (IXLRow row in wshRows)
            {
                IXLCells wshCells = row.Cells("1:3");

                int id = Convert.ToInt32(row.Cell(1).Value);

                // Создаём и добавляем элементы.
                версииДанных.Add(
                    id,
                    new ВерсияДанных
                    {
                        ID = id,
                        Name = row.Cell(2).Value.ToString(),
                        VersionType = row.Cell(3).Value.ToString(),
                    });
            }

            return версииДанных;
        }
    }
}
