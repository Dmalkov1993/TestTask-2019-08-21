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
            using (var client = new HttpClient())
            {
                // Запрашиваем весь справочник "Объекты Строительства"
                var response = client.GetAsync(directoryServiceUrl + "/Construction").Result;
                string результатЗапроса = response.Content.ReadAsStringAsync().Result;

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

            return View("~/Views/ReportView.cshtml");
        }

        // GET: api/Report/5
        [HttpGet("{id}", Name = "GetConstruction")]
        public string GetConstruction(int constructionId, int dataVersionId)
        {
            return "value";
        }
    }
}
