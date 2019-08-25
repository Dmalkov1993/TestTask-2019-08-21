using Newtonsoft.Json;
using ReportService.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    /// <summary>
    /// Утилитарный класс, тут всё, что не относится к перехватыванию запроса и генерации отчёта.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Метод содержит конструкции, по созданию всех возможных вариаций запросов к сервису справочников (DirectoryService)
        /// </summary>
        public static async Task TestDirectoryServiceApiAsync(UrlSettings urlSettings)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;

                // Запросим и десериализуем метаданные по справочникам:
                // Для справочника "Объекты Строительства"
                response = await client.GetAsync($"{urlSettings.ConstructionObjectUrl}GetMetadata");
                DirectoryMetadata MetadataDirectoryOfConstructionObjects = await response.Content.ReadAsAsync<DirectoryMetadata>();

                // Для справочника "Версии Данных"
                response = await client.GetAsync($"{urlSettings.DataVersionUrl}GetMetadata");
                DirectoryMetadata MetadataDirectoryOfDataVersions = await response.Content.ReadAsAsync<DirectoryMetadata>();


                // TODO: Полученные метаданные можно сравнивать с метаданными объектов этого сервиса
                // и в случае несовпадения, как-то уведомлять разработчиков о необх. поправки структуры классов.



                // Запрашиваем весь справочник "Объекты Строительства"
                response = await client.GetAsync($"{urlSettings.ConstructionObjectUrl}GetAllDirectory");
                IEnumerable<ConstructionObject> сonstructionObjects = await response.Content.ReadAsAsync<IEnumerable<ConstructionObject>>();

                // Запрашиваем весь справочник "Версии Данных"
                response = await client.GetAsync($"{urlSettings.DataVersionUrl}GetAllDirectory");
                IEnumerable<DataVersion> dataVersions = await response.Content.ReadAsAsync<IEnumerable<DataVersion>>();

                // Запросим Объект Строительства с идом = 2:
                int indexOfConstructionObject = 2;

                response = await client.GetAsync($"{urlSettings.ConstructionObjectUrl}Elements/{indexOfConstructionObject}");
                ConstructionObject сonstructionObject = await response.Content.ReadAsAsync<ConstructionObject>();


                // Запросим Версию Данных с идом = 3:
                int indexOfDataVersion = 3;

                response = await client.GetAsync($"{urlSettings.DataVersionUrl}Elements/{indexOfConstructionObject}");
                DataVersion dataVersion = await response.Content.ReadAsAsync<DataVersion>();


                // Попробуем запросить Версию Данных с идом = 88:
                indexOfDataVersion = 88;

                response = await client.GetAsync($"{urlSettings.DataVersionUrl}Elements/{indexOfDataVersion}");

                // dataVersionEmpty будет = null.
                DataVersion dataVersionEmpty = await response.Content.ReadAsAsync<DataVersion>();
            }
        }

        /// <summary>
        /// Метод по созданию файла конфигурации отчёта.
        /// </summary>
        public static void CreateReportSetting()
        {
            List<ReportSetting> ReportSettings = new List<ReportSetting>();

            for (int confID = 1; confID < 4; confID++)
            {
                ReportSetting reportSetting = new ReportSetting();

                reportSetting.ConfigurationID = confID;

                reportSetting.DirectoryIdOnRows = 1; // Берём "Объекты строительства"
                reportSetting.DirectoryIdOnColumns = 2; // Берём "Версии данных"

                // По заданию - справочник по строкам – «Объекты строительства» с атрибутами «Код объекта» и «Наименование»
                reportSetting.AttributesOfDirectoryOnRows.Add("Code");
                reportSetting.AttributesOfDirectoryOnRows.Add("Name");

                // По заданию - справочник по столбцам – «Версии данных» (возьмём только атрибут "Наименование", чтобы вывести столбцы Версия 1 Версия 2 Версия 3)
                reportSetting.AttributesOfDirectoryOnColumns.Add("Name");

                // Добавляем в коллекцию настроек
                ReportSettings.Add(reportSetting);
            }

            // Сериализуем и в файлик "ReportSettings-1.json", где 1 - пусть будет номер конфигурации
            string queryListForWriting = JsonConvert.SerializeObject(ReportSettings);

            using (StreamWriter sw = new StreamWriter("ReportSettings_new.json", false, Encoding.UTF8))
            {
                sw.WriteLine(queryListForWriting);
            }
        }
    }
}
