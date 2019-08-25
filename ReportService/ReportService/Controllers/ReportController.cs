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
using Microsoft.Extensions.Options;
using ReportService.ReportBuiders;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        public ReportController(IOptions<List<ReportSetting>> reportSettings, IOptions<UrlSettings> urlSettings)
        {
            // Подхватываем настройки для отчётов
            this.reportSettings = reportSettings.Value;

            // Подхватываем наши URL-ы
            this.urlSettings = urlSettings.Value;
        }

        /// <summary>
        /// Конфигурации отчётов из appsettings.json.
        /// </summary>
        private List<ReportSetting> reportSettings { get; set; }

        /// <summary>
        /// Конфигурации URL-ов из appsettings.json.
        /// </summary>
        private UrlSettings urlSettings { get; set; }

        // Файл настроек отчёта согласно заданию - "ReportSettings-1.json"
        // public const string fileWithSettingsForReport = "ReportSettings-3-All.json"; // Можно и "ReportSettings-2.json" и "ReportSettings-3-All.json"

        // GET: api/Report
        [HttpGet]
        [HttpGet("{reportSettingID}", Name = "MakeReport")]
        public async Task<ActionResult> GetAsync(int reportSettingID = 1)
        {
            TestTaskReportBuilder testTaskReportBuilder = new TestTaskReportBuilder();

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
                        await Utils.TestDirectoryServiceApiAsync(urlSettings);
                    }

                    // Запрашиваем весь справочник "Объекты Строительства"
                    response = await client.GetAsync($"{urlSettings.ConstructionObjectUrl}GetAllDirectory");
                    constructionObjects = await response.Content.ReadAsAsync<IEnumerable<ConstructionObject>>();

                    // Запрашиваем весь справочник "Версии данных"
                    response = await client.GetAsync($"{urlSettings.DataVersionUrl}GetAllDirectory");
                    dataVersions = await response.Content.ReadAsAsync<IEnumerable<DataVersion>>();
                }

                // Берём из настроек конфигурацию отчёта
                ReportSetting reportSetting = new ReportSetting();

                try
                {
                    var reportSettingCol = reportSettings.Where(set => set.ConfigurationID == reportSettingID);

                    if (reportSettingCol.Any())
                    {
                        reportSetting = reportSettingCol.First();
                    }
                    else
                    {
                        // Если настойка не найдена, говорим 404.
                        return this.StatusCode(404);
                    }
                }
                catch (Exception ex) { }

                // Билдим отчёт.
                DataTable report = testTaskReportBuilder.BuildReport(reportSetting, constructionObjects, dataVersions);

                return View("~/Views/ReportView.cshtml", report);
            }
            catch (Exception ex)
            {
                // Что-то пошло не так, 
                return this.StatusCode(500);

                // TODO: пишем в логи ошибку
            }
        }
    }
}
