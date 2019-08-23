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

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        public const string directoryServiceUrl = "https://localhost:44366/api";

        // GET: api/Report
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response;
                    string результатЗапроса = string.Empty;

                    bool IsNeedtest = true;

                    if (IsNeedtest)
                    {
                        TestDirectoryServiceApi();
                    }

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = client.GetAsync(directoryServiceUrl + "/Construction").Result;
                    результатЗапроса = response.Content.ReadAsStringAsync().Result;

                    IEnumerable<ОбъектСтроительства> объектыСтроительства = JsonConvert.DeserializeObject<IEnumerable<ОбъектСтроительства>>(результатЗапроса);

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = client.GetAsync(directoryServiceUrl + "/DataVersion").Result;
                    результатЗапроса = response.Content.ReadAsStringAsync().Result;

                    IEnumerable<ВерсияДанных> версииДанных = JsonConvert.DeserializeObject<IEnumerable<ВерсияДанных>>(результатЗапроса);

                    // Читаем настройки отчёта. В силу упрощения задания - без явного указания 
                    // СоздатьНастройкуОтчёта();

                    НастройкаОтчёта настройкаОтчёта;

                    using (StreamReader reader = new StreamReader("ReportSettings\\ReportSettings-1.json", Encoding.UTF8))
                    {
                        string jsonПредставление = reader.ReadToEnd();
                        настройкаОтчёта = JsonConvert.DeserializeObject<НастройкаОтчёта>(jsonПредставление);
                    }


                }

                return View("~/Views/ReportView.cshtml");
            }
            catch (Exception ex)
            {
                // Что-то пошло не так, 
                return this.StatusCode(500);

                // TODO: пишем в логи ошибку
            }
        }




        // GET: api/Report/5
        [HttpGet("{id}", Name = "GetConstruction")]
        public string GetConstruction(int constructionId, int dataVersionId)
        {
            return "value";
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
