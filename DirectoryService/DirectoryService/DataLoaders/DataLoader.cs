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
        public static Dictionary<int, ConstructionObject> GetDataOfConstructionObjectsDirectory()
        {
            // Прочитаем Excel файл с данными:
            Dictionary<int, ConstructionObject> objectsOfBuilding = new Dictionary<int, ConstructionObject>();

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
                objectsOfBuilding.Add(
                    id,
                    new ConstructionObject
                    {
                        ID = id,
                        Name = row.Cell(2).Value.ToString(),
                        Code = row.Cell(3).Value.ToString(),
                        Budget = (double)row.Cell(4).Value,
                    });
            }

            return objectsOfBuilding;
        }

        /// <summary>
        /// Метод по получению списка объектов справочника "ОбъектыСтроительства".
        /// </summary>
        /// <returns>Список объектов справочника "ОбъектыСтроительства".</returns>
        public static Dictionary<int, DataVersion> GetDataOfDataVersionDirectory()
        {
            // Прочитаем Excel файл с данными:
            Dictionary<int, DataVersion> dataVersions = new Dictionary<int, DataVersion>();

            // Открываем файл
            XLWorkbook workBook = new XLWorkbook("DataSources\\DataVersions.xlsx");
            IXLWorksheet workSheet = workBook.Worksheet(1);

            int indexOfLastRow = workSheet.RangeUsed().LastRowUsed().RangeAddress.FirstAddress.RowNumber;

            // Читаем со второй строки, так как первая это шапка-заголовок
            var wshRows = workSheet.Rows(2, indexOfLastRow);

            foreach (IXLRow row in wshRows)
            {
                IXLCells wshCells = row.Cells("1:3");

                int id = Convert.ToInt32(row.Cell(1).Value);

                // Создаём и добавляем элементы.
                dataVersions.Add(
                    id,
                    new DataVersion
                    {
                        ID = id,
                        Name = row.Cell(2).Value.ToString(),
                        VersionType = row.Cell(3).Value.ToString(),
                    });
            }

            return dataVersions;
        }
    }
}
