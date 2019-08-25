using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using ReportService.Objects;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        public const string constructionObjectUrl = "https://localhost:44366/api/ConstructionObjects/";
        public const string DataVersionUrl = "https://localhost:44366/api/DataVersions/";

        // Файл настроек отчёта согласно заданию - "ReportSettings-1.json"
        public const string fileWithSettingsForReport = "ReportSettings-3-All.json"; // Можно и "ReportSettings-2.json" и "ReportSettings-3-All.json"

        // GET: api/Report
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                IEnumerable<ConstructionObject> constructionObjects;
                IEnumerable<DataVersion> dataVersions;

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response;
                    string requestResult = string.Empty;

                    bool IsNeedtest = false;

                    if (IsNeedtest)
                    {
                        // Этот метод делает все воможные запросы к сервису DirectoryService (чисто в рамках демонстрации)
                        TestDirectoryServiceApi();
                    }

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = client.GetAsync($"{constructionObjectUrl}GetAllDirectory").Result;
                    constructionObjects = response.Content.ReadAsAsync<IEnumerable<ConstructionObject>>().Result;

                    // constructionObjects = JsonConvert.DeserializeObject<IEnumerable<ConstructionObject>>(requestResult);

                    // Запрашиваем весь справочник "Версии данных"
                    response = client.GetAsync($"{DataVersionUrl}GetAllDirectory").Result;
                    dataVersions = response.Content.ReadAsAsync<IEnumerable<DataVersion>>().Result;

                    // dataVersions = JsonConvert.DeserializeObject<IEnumerable<DataVersion>>(requestResult);
                }
                // Читаем настройки отчёта. В силу упрощения задания - без явного указания 
                // СоздатьНастройкуОтчёта();

                ReportSetting reportSetting;

                using (StreamReader reader = new StreamReader($"ReportSettings\\{fileWithSettingsForReport}", Encoding.UTF8))
                {
                    string json = reader.ReadToEnd();
                    reportSetting = JsonConvert.DeserializeObject<ReportSetting>(json);
                }

                // Строим отчёт. Считаем, что в настройках разработчики указали верный набор параметров атрибутов для справочников.

                DataTable report = new DataTable();

                string directoryNameByColumns = GetDirectoryNameByID(reportSetting.DirectoryIdOnColumns);
                string directoryNameByRows = GetDirectoryNameByID(reportSetting.DirectoryIdOnRows);

                // Добавим столбцы к таблице отчёта. 
                // Учтём, что у двух справочников могут быть одни и те же имена атрибутов, и добавим префикс в виде имени справочника.
                reportSetting.AttributesOfDirectoryOnRows.ForEach(attr =>
                {
                    report.Columns.Add($"{directoryNameByRows}-{attr}");
                });

                // Если по столбцам, надо вставить все поля указанного атрибута в таблицу как столбец:
                reportSetting.AttributesOfDirectoryOnColumns.ForEach(attr =>
                {
                    // Смотрим, из какого справочника надо взять данные
                    if (reportSetting.DirectoryIdOnColumns == 1)
                    {
                        // Если 1, берём объекты строительства
                        AddDirectoryColumnToReport(constructionObjects, attr, ref report);
                    }
                    else if (reportSetting.DirectoryIdOnColumns == 2)
                    {
                        // Если 2, берём версииДанных
                        AddDirectoryColumnToReport(dataVersions, attr, ref report);
                    }
                });

                // Добавим данные в таблицу отчёта. Принцип тот же - проверим какой справочник как надо загрузить
                if (reportSetting.DirectoryIdOnRows == 1)
                {
                    // Если 1, берём объекты строительства в строки отчёта
                    AddRowsToReport(constructionObjects, directoryNameByRows, reportSetting, ref report);
                }
                else if (reportSetting.DirectoryIdOnRows == 2)
                {
                    // Если 2, берём версииДанных
                    AddRowsToReport(dataVersions, directoryNameByRows, reportSetting, ref report);
                }

                // Теперь пройдёмся по таблице отчёта, и внесём в отчёт иды другого транспонированного справочника:
                if (reportSetting.DirectoryIdOnColumns == 1)
                {
                    // Если 1, берём Иды строительства в строки отчёта
                    AddIDsToReport(constructionObjects, directoryNameByColumns, reportSetting, ref report);
                }
                else if (reportSetting.DirectoryIdOnColumns == 2)
                {
                    // Если 2, берём берём Иды версииДанных
                    AddIDsToReport(dataVersions, directoryNameByColumns, reportSetting, ref report);
                }

                // Уберём лишнее из названий заголовков таблицы
                foreach (DataColumn column in report.Columns)
                {
                    if (column.ColumnName.Contains("-"))
                    {
                        column.ColumnName = column.ColumnName.Split("-")[1];
                    }
                }

                return View("~/Views/ReportView.cshtml", report);
            }
            catch (Exception ex)
            {
                // Что-то пошло не так, 
                return this.StatusCode(500);

                // TODO: пишем в логи ошибку
            }
        }

        private void AddIDsToReport(IEnumerable<object> directory, string directoryNameByColumns, ReportSetting reportSetting, ref DataTable report)
        {
            foreach (object record in directory)
            {
                // Получим ИД данной записи в справочнике
                int id = (int)GetObjectPropertyValueByAttrName(record, "ID");

                foreach (string attr in reportSetting.AttributesOfDirectoryOnColumns)
                {
                    string columnName = Convert.ToString(GetObjectPropertyValueByAttrName(record, attr));

                    foreach (DataRow recordReport in report.Rows)
                    {
                        // Проапдейтим формат:
                        // "<первая буква справочника по строкам><ид записи справочника по строкам> - <первая буква справочника по столбцам><ид записи справочника по столбцам>"
                        recordReport[columnName] += $" - {directoryNameByColumns.First()}{id}";
                    }
                }
            }
        }

        private void AddRowsToReport(IEnumerable<object> directory, string directoryName, ReportSetting reportSetting, ref DataTable report)
        {
            foreach (object record in directory)
            {
                DataRow rowForAdd = report.NewRow();

                foreach (string attr in reportSetting.AttributesOfDirectoryOnRows)
                {
                    // Берём указанный в настройках атрибут
                    rowForAdd[$"{directoryName}-{attr}"] = GetObjectPropertyValueByAttrName(record, attr);
                }

                // Получим ИД данной записи в справочнике
                int id = (int)GetObjectPropertyValueByAttrName(record, "ID");

                // Добавим в столбцы транспонированной матрицы идентификаторы строки, с указанием того, что это Иды этого справочника
                int startCol = reportSetting.AttributesOfDirectoryOnRows.Count;
                int endCol = report.Columns.Count;

                for (int index = startCol; index < endCol; index++)
                {
                    // Формат строк транспонированной матрицы "<первая буква справочника><ид записи справочника>"
                    rowForAdd[index] = $"{directoryName.First()}{id}";
                }

                report.Rows.Add(rowForAdd);
            }
        }

        /// <summary>
        /// TODO: Надо проверить, кидается ли исключение, если такого св-ва у объекта не нашлось.
        /// TODO: в MyUtils добавить. Может в других проектах пригодится.
        /// </summary>
        /// <param name="readedObject"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        private object GetObjectPropertyValueByAttrName(object readedObject, string attrName)
        {
            PropertyInfo pi = readedObject.GetType().GetProperty(attrName);

            if (pi == null)
            {
                throw new Exception("Запрашиваемое свойство не найдено!");
            }

            return pi.GetValue(readedObject, null);
        }

        /// <summary>
        /// Метод по добавлению в <see cref="DataTable"/> коллекцию столбцов, которая является коллекцией значений <see cref="string"/> справочника.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="attr"></param>
        /// <param name="report"></param>
        private void AddDirectoryColumnToReport(IEnumerable<object> directory, string attr, ref DataTable report)
        {
            foreach (object record in directory)
            {
                // Добавляем столбец
                try
                {
                    // Может оказаться, что не все поля в столбце будут уникальными, поэтому, пропускаем такие столбцы
                    report.Columns.Add(Convert.ToString(GetObjectPropertyValueByAttrName(record, attr)));
                }
                catch { } // Ничего не делаем, т.к. ошибка скорее всего несерьёзная
            }
        }

        /// <summary>
        /// Метод-костыль, так как только на финальном этапе реализации ReportService-а понял, что надо было делать запрос справочника по его Ид
        /// TODO: сделать метод по возвращению данных обоих справочников по ид (1 или 2)
        /// </summary>
        /// <param name="directoryID"></param>
        /// <returns></returns>
        private string GetDirectoryNameByID(int directoryID)
        {
            string directoryName = string.Empty;

            if (directoryID == 1)
            {
                directoryName = "ОбъектыСтроительства";
            }
            else if (directoryID == 2)
            {
                directoryName = "ВерсииДанных";
            }
            else
            {
                throw new Exception($"Не найден справочник с ид {directoryID}");
            }

            return directoryName;
        }

        /// <summary>
        /// Метод содержит конструкции, по созданию всех возможных вариаций запросов к сервису справочников (DirectoryService)
        /// </summary>
        private void TestDirectoryServiceApi()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;

                // Запросим и десериализуем метаданные по справочникам:
                // Для справочника "Объекты Строительства"
                response = client.GetAsync($"{constructionObjectUrl}GetMetadata").Result;
                DirectoryMetadata MetadataDirectoryOfConstructionObjects = response.Content.ReadAsAsync<DirectoryMetadata>().Result;
                
                // Для справочника "Версии Данных"
                response = client.GetAsync($"{DataVersionUrl}GetMetadata").Result;
                DirectoryMetadata MetadataDirectoryOfDataVersions = response.Content.ReadAsAsync<DirectoryMetadata>().Result;


                // TODO: Полученные метаданные можно сравнивать с метаданными объектов этого сервиса
                // и в случае несовпадения, как-то уведомлять разработчиков о необх. поправки структуры классов.



                // Запрашиваем весь справочник "Объекты Строительства"
                response = client.GetAsync($"{constructionObjectUrl}GetAllDirectory").Result;
                IEnumerable<ConstructionObject> сonstructionObjects = response.Content.ReadAsAsync<IEnumerable<ConstructionObject>>().Result;
                
                // Запрашиваем весь справочник "Версии Данных"
                response = client.GetAsync($"{DataVersionUrl}GetAllDirectory").Result;
                IEnumerable<DataVersion> dataVersions = response.Content.ReadAsAsync<IEnumerable<DataVersion>>().Result;
                
                // Запросим Объект Строительства с идом = 2:
                int indexOfConstructionObject = 2;

                response = client.GetAsync($"{constructionObjectUrl}Elements/{indexOfConstructionObject}").Result;
                ConstructionObject сonstructionObject = response.Content.ReadAsAsync<ConstructionObject>().Result;


                // Запросим Версию Данных с идом = 3:
                int indexOfDataVersion = 3;

                response = client.GetAsync($"{DataVersionUrl}Elements/{indexOfConstructionObject}").Result;
                DataVersion dataVersion = response.Content.ReadAsAsync<DataVersion>().Result;


                // Попробуем запросить Версию Данных с идом = 88:
                indexOfDataVersion = 88;

                response = client.GetAsync($"{DataVersionUrl}Elements/{indexOfDataVersion}").Result;

                // dataVersionEmpty будет = null.
                DataVersion dataVersionEmpty = response.Content.ReadAsAsync<DataVersion>().Result;   
            }
        }

        /// <summary>
        /// Метод по созданию файла конфигурации отчёта.
        /// </summary>
        private void CreateReportSetting()
        {
            ReportSetting reportSetting = new ReportSetting();

            reportSetting.DirectoryIdOnRows = 1; // Берём "Объекты строительства"
            reportSetting.DirectoryIdOnColumns = 2; // Берём "Версии данных"

            // По заданию - справочник по строкам – «Объекты строительства» с атрибутами «Код объекта» и «Наименование»
            reportSetting.AttributesOfDirectoryOnRows.Add("Code");
            reportSetting.AttributesOfDirectoryOnRows.Add("Name");

            // По заданию - справочник по столбцам – «Версии данных» (возьмём только атрибут "Наименование", чтобы вывести столбцы Версия 1 Версия 2 Версия 3)
            reportSetting.AttributesOfDirectoryOnColumns.Add("Name");

            // Сериализуем и в файлик "ReportSettings-1.json", где 1 - пусть будет номер конфигурации
            string queryListForWriting = JsonConvert.SerializeObject(reportSetting);

            using (StreamWriter sw = new StreamWriter("ReportSettings-1.json", false, Encoding.UTF8))
            {
                sw.WriteLine(queryListForWriting);
            }
        }
    }
}
