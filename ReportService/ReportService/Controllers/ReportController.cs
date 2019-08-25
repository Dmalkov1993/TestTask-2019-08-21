namespace ReportService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using ReportService.Objects;
    using System.Data;
    using Microsoft.Extensions.Options;
    using ReportService.ReportBuiders;

    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        public ReportController(IOptions<List<ReportSetting>> reportSettings, IOptions<UrlSettings> urlSettings, IReportBuilder reportBuilder)
        {
            // Подхватываем настройки для отчётов
            this.reportSettings = reportSettings.Value;

            // Подхватываем наши URL-ы
            this.urlSettings = urlSettings.Value;

            // Подхватываем билдер отчёта, указанный в Startup-е
            this.reportBuilder = reportBuilder;
        }

        /// <summary>
        /// Конфигурации отчётов из appsettings.json.
        /// </summary>
        private List<ReportSetting> reportSettings { get; set; }

        /// <summary>
        /// Конфигурации URL-ов из appsettings.json.
        /// </summary>
        private UrlSettings urlSettings { get; set; }

        /// <summary>
        /// Наш ReportBuilder, подхваченный из DI.
        /// </summary>
        private IReportBuilder reportBuilder;
    
        [HttpGet]
        [HttpGet("{reportSettingID}", Name = "MakeReport")]
        public async Task<ActionResult> GetAsync(int reportSettingID = 1)
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
                DataTable report = reportBuilder.BuildReport(reportSetting, constructionObjects, dataVersions);

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
