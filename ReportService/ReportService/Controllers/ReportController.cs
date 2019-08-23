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
        public const string directoryServiceUrl = "https://localhost:44366/api";

        // Файл настроек отчёта согласно заданию - "ReportSettings-1.json"
        public const string файлНастроек = "ReportSettings-1.json"; // Можно и "ReportSettings-2.json" и "ReportSettings-3-All.json"

        // GET: api/Report
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                IEnumerable<ОбъектСтроительства> объектыСтроительства;
                IEnumerable<ВерсияДанных> версииДанных;

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response;
                    string результатЗапроса = string.Empty;

                    bool IsNeedtest = false;

                    if (IsNeedtest)
                    {
                        // Этот метод делает все воможные запросы к сервису DirectoryService (чисто в рамках демонстрации)
                        TestDirectoryServiceApi();
                    }

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = client.GetAsync(directoryServiceUrl + "/Construction").Result;
                    результатЗапроса = response.Content.ReadAsStringAsync().Result;

                    объектыСтроительства = JsonConvert.DeserializeObject<IEnumerable<ОбъектСтроительства>>(результатЗапроса);

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = client.GetAsync(directoryServiceUrl + "/DataVersion").Result;
                    результатЗапроса = response.Content.ReadAsStringAsync().Result;

                    версииДанных = JsonConvert.DeserializeObject<IEnumerable<ВерсияДанных>>(результатЗапроса);
                }
                // Читаем настройки отчёта. В силу упрощения задания - без явного указания 
                // СоздатьНастройкуОтчёта();

                НастройкаОтчёта настройкаОтчёта;

                using (StreamReader reader = new StreamReader($"ReportSettings\\{файлНастроек}", Encoding.UTF8))
                {
                    string jsonПредставление = reader.ReadToEnd();
                    настройкаОтчёта = JsonConvert.DeserializeObject<НастройкаОтчёта>(jsonПредставление);
                }

                // Строим отчёт. Считаем, что в настройках разработчики указали верный набор параметров атрибутов для справочников.

                DataTable отчёт = new DataTable();

                string справочникСтолбцы = ПолучитьИмяСправочникаПоЕгоИД(настройкаОтчёта.ID_DirectoryOnColumns);
                string справочникСтроки = ПолучитьИмяСправочникаПоЕгоИД(настройкаОтчёта.ID_DirectoryOnRows);

                // Добавим столбцы к таблице отчёта. 
                // Учтём, что у двух справочников могут быть одни и те же имена атрибутов, и добавим префикс в виде имени справочника.
                настройкаОтчёта.AttributesOfDirectoryOnRows.ForEach(attr =>
                {
                    отчёт.Columns.Add($"{справочникСтроки}-{attr}");
                });

                // Если по столбцам, надо вставить все поля указанного атрибута в таблицу как столбец:
                настройкаОтчёта.AttributesOfDirectoryOnColumns.ForEach(attr =>
                {
                    // Смотрим, из какого справочника надо взять данные
                    if (настройкаОтчёта.ID_DirectoryOnColumns == 1)
                    {
                        // Если 1, берём объекты строительства
                        ДобавитьВОтчётСтолбецСправочника(объектыСтроительства, attr, ref отчёт);
                    }
                    else if (настройкаОтчёта.ID_DirectoryOnColumns == 2)
                    {
                        // Если 2, берём версииДанных
                        ДобавитьВОтчётСтолбецСправочника(версииДанных, attr, ref отчёт);
                    }
                });

                // Добавим данные в таблицу отчёта. Принцип тот же - проверим какой справочник как надо загрузить
                if (настройкаОтчёта.ID_DirectoryOnRows == 1)
                {
                    // Если 1, берём объекты строительства в строки отчёта
                    ДобавитьВОтчётСтроки(объектыСтроительства, справочникСтроки, настройкаОтчёта, ref отчёт);
                }
                else if (настройкаОтчёта.ID_DirectoryOnRows == 2)
                {
                    // Если 2, берём версииДанных
                    ДобавитьВОтчётСтроки(версииДанных, справочникСтроки, настройкаОтчёта, ref отчёт);
                }

                // Теперь пройдёмся по таблице отчёта, и внесём в отчёт иды другого транспонированного справочника:
                if (настройкаОтчёта.ID_DirectoryOnColumns == 1)
                {
                    // Если 1, берём Иды строительства в строки отчёта
                    ДобавитьВОтчётИдентификаторы(объектыСтроительства, справочникСтолбцы, настройкаОтчёта, ref отчёт);
                }
                else if (настройкаОтчёта.ID_DirectoryOnColumns == 2)
                {
                    // Если 2, берём берём Иды версииДанных
                    ДобавитьВОтчётИдентификаторы(версииДанных, справочникСтолбцы, настройкаОтчёта, ref отчёт);
                }

                // Уберём лишнее из названий заголовков таблицы
                foreach (DataColumn столбец in отчёт.Columns)
                {
                    if (столбец.ColumnName.Contains("-"))
                    {
                        столбец.ColumnName = столбец.ColumnName.Split("-")[1];
                    }
                }

                return View("~/Views/ReportView.cshtml", отчёт);
            }
            catch (Exception ex)
            {
                // Что-то пошло не так, 
                return this.StatusCode(500);

                // TODO: пишем в логи ошибку
            }
        }

        private void ДобавитьВОтчётИдентификаторы(IEnumerable<object> справочник, string справочникСтолбцы, НастройкаОтчёта настройкаОтчёта, ref DataTable отчёт)
        {
            foreach (object запись in справочник)
            {
                // Получим ИД данной записи в справочнике
                int id = (int)ПолучитьЗначениеСвойстваОбъекта(запись, "ID");

                foreach (string атрибут in настройкаОтчёта.AttributesOfDirectoryOnColumns)
                {
                    string имяСтолбца = Convert.ToString(ПолучитьЗначениеСвойстваОбъекта(запись, атрибут));

                    foreach (DataRow записьОтчёта in отчёт.Rows)
                    {
                        // Проапдейтим формат:
                        // "<первая буква справочника по строкам><ид записи справочника по строкам> - <первая буква справочника по столбцам><ид записи справочника по столбцам>"
                        записьОтчёта[имяСтолбца] += $" - {справочникСтолбцы.First()}{id}";
                    }
                }
            }
        }

        private void ДобавитьВОтчётСтроки(IEnumerable<БазоваяЗаписьСправочника> справочник, string справочникСтроки, НастройкаОтчёта настройкаОтчёта, ref DataTable отчёт)
        {
            foreach (object запись in справочник)
            {
                DataRow строка = отчёт.NewRow();

                foreach (string атрибут in настройкаОтчёта.AttributesOfDirectoryOnRows)
                {
                    // Берём указанный в настройках атрибут
                    строка[$"{справочникСтроки}-{атрибут}"] = ПолучитьЗначениеСвойстваОбъекта(запись, атрибут);
                }

                // Получим ИД данной записи в справочнике
                int id = (int)ПолучитьЗначениеСвойстваОбъекта(запись, "ID");

                // Добавим в столбцы транспонированной матрицы идентификаторы строки, с указанием того, что это Иды этого справочника
                int startCol = настройкаОтчёта.AttributesOfDirectoryOnRows.Count;
                int endCol = отчёт.Columns.Count;

                for (int index = startCol; index < endCol; index++)
                {
                    // Формат строк транспонированной матрицы "<первая буква справочника><ид записи справочника>"
                    строка[index] = $"{справочникСтроки.First()}{id}";
                }

                отчёт.Rows.Add(строка);
            }
        }

        /// <summary>
        /// TODO: Надо проверить, кидается ли исключение, если такого св-ва у объекта не нашлось.
        /// TODO: в MyUtils добавить. Может в других проектах пригодиться.
        /// </summary>
        /// <param name="объект"></param>
        /// <param name="имяСвойства"></param>
        /// <returns></returns>
        private object ПолучитьЗначениеСвойстваОбъекта(object объект, string имяСвойства)
        {
            PropertyInfo pi = объект.GetType().GetProperty(имяСвойства);

            if (pi == null)
            {
                throw new Exception("Запрашиваемое свойство не найдено!");
            }

            return pi.GetValue(объект, null);
        }

        /// <summary>
        /// Метод по добавлению в <see cref="DataTable"/> коллекцию столбцов, которая является коллекцией значений <see cref="string"/> справочника.
        /// </summary>
        /// <param name="справочник"></param>
        /// <param name="attr"></param>
        /// <param name="отчёт"></param>
        private void ДобавитьВОтчётСтолбецСправочника(IEnumerable<БазоваяЗаписьСправочника> справочник, string attr, ref DataTable отчёт)
        {
            foreach (object запись in справочник)
            {
                // Добавляем столбец
                try
                {
                    // Может оказаться, что не все поля в столбце будут уникальными, поэтому, пропускаем такие столбцы
                    отчёт.Columns.Add(Convert.ToString(ПолучитьЗначениеСвойстваОбъекта(запись, attr)));
                }
                catch { } // Ничего не делаем, т.к. ошибка скорее всего несерьёзная
            }
        }

        /// <summary>
        /// Метод-костыль, так как только на финальном этапе реализации ReportService-а понял, что надо было делать запрос справочника по его Ид
        /// TODO: сделать метод по возвращению данных обоих справочников по ид (1 или 2)
        /// </summary>
        /// <param name="идентификаторСправочника"></param>
        /// <returns></returns>
        private string ПолучитьИмяСправочникаПоЕгоИД(int идентификаторСправочника)
        {
            string имяСправочника = string.Empty;

            if (идентификаторСправочника == 1)
            {
                имяСправочника = "ОбъектыСтроительства";
            }
            else if (идентификаторСправочника == 2)
            {
                имяСправочника = "ВерсииДанных";
            }

            return имяСправочника;
        }

        /// <summary>
        /// Метод содержит конструкции, по созданию всех возможных вариаций запросов к сервису справочников (DirectoryService)
        /// </summary>
        private void TestDirectoryServiceApi()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                string результатЗапроса = string.Empty;

                // Запросим и десериализуем метаданные по справочникам:
                // Для справочника "Объекты Строительства"
                response = client.GetAsync(directoryServiceUrl + "/Construction?getMeta=true").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;
                МетаданныеСправочника метаданныеСпрОбъектыСтроительства = JsonConvert.DeserializeObject<МетаданныеСправочника>(результатЗапроса);

                // Для справочника "Версии Данных"
                response = client.GetAsync(directoryServiceUrl + "/DataVersion?getMeta=true").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;
                МетаданныеСправочника метаданныеСпрВерсииДанных = JsonConvert.DeserializeObject<МетаданныеСправочника>(результатЗапроса);

                // TODO: Полученные метаданные можно сравнивать с метаданными объектов этого сервиса
                // и в случае несовпадения, как-то уведомлять разработчиков о необх. поправки структуры классов.



                // Запрашиваем весь справочник "Объекты Строительства"
                response = client.GetAsync(directoryServiceUrl + "/Construction").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;

                IEnumerable<ОбъектСтроительства> объектыСтроительства = JsonConvert.DeserializeObject<IEnumerable<ОбъектСтроительства>>(результатЗапроса);

                // Запрашиваем весь справочник "Объекты Строительства"
                response = client.GetAsync(directoryServiceUrl + "/DataVersion").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;

                IEnumerable<ВерсияДанных> версииДанных = JsonConvert.DeserializeObject<IEnumerable<ВерсияДанных>>(результатЗапроса);




                // Запросим Объект Строительства с идом = 2:
                int индексОбъектаСтроительства = 2;

                response = client.GetAsync(directoryServiceUrl + $"/Construction/{индексОбъектаСтроительства}").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;

                ОбъектСтроительства объектСтроительства = JsonConvert.DeserializeObject<ОбъектСтроительства>(результатЗапроса);

                // Запросим Версию Данных с идом = 3:
                int индексВерсииДанных = 3;

                response = client.GetAsync(directoryServiceUrl + $"/DataVersion/{индексВерсииДанных}").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;

                ВерсияДанных версияДанных = JsonConvert.DeserializeObject<ВерсияДанных>(результатЗапроса);

                // Попробуем запросить Версию Данных с идом = 88:
                индексВерсииДанных = 88;

                response = client.GetAsync(directoryServiceUrl + $"/DataVersion/{индексВерсииДанных}").Result;
                результатЗапроса = response.Content.ReadAsStringAsync().Result;

                // версияДанных будет = null после десериализации.
                версияДанных = JsonConvert.DeserializeObject<ВерсияДанных>(результатЗапроса);
            }
        }

        /// <summary>
        /// Метод по созданию файла конфигурации отчёта.
        /// </summary>
        private void СоздатьНастройкуОтчёта()
        {
            НастройкаОтчёта настройкаОтчёта = new НастройкаОтчёта();

            настройкаОтчёта.ID_DirectoryOnRows = 1; // Берём "Объекты строительства"
            настройкаОтчёта.ID_DirectoryOnColumns = 2; // Берём "Версии данных"

            // По заданию - справочник по строкам – «Объекты строительства» с атрибутами «Код объекта» и «Наименование»
            настройкаОтчёта.AttributesOfDirectoryOnRows.Add("Code");
            настройкаОтчёта.AttributesOfDirectoryOnRows.Add("Name");

            // По заданию - справочник по столбцам – «Версии данных» (возьмём только атрибут "Наименование", чтобы вывести столбцы Версия 1 Версия 2 Версия 3)
            настройкаОтчёта.AttributesOfDirectoryOnColumns.Add("Name");

            // Сериализуем и в файлик "ReportSettings-1.json", где 1 - пусть будет номер конфигурации
            string queryListForWriting = JsonConvert.SerializeObject(настройкаОтчёта);

            using (StreamWriter sw = new StreamWriter("ReportSettings-1.json", false, Encoding.UTF8))
            {
                sw.WriteLine(queryListForWriting);
            }
        }
    }
}
