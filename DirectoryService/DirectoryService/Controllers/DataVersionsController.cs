namespace DirectoryService.Controllers
{
    using System;
    using System.Collections.Generic;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Mvc;

    // [Route("api/DataVersion")]
    [ApiController]
    public class DataVersionsController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Версии данных".
        /// </summary>
        /// <returns></returns>
        [Route("api/DataVersions/GetAllDirectory")]
        [HttpGet]
        public ActionResult<Dictionary<int, DataVersion>.ValueCollection> Get()
        {
            // TODO: можно подобную конструкцию в контроллере DataVersionController вынести в метод базового контроллера
            try
            {
                return DataLoader.GetDataOfDataVersionDirectory().Values;
            }
            catch (Exception ex)
            {
                // Отлавливается исключение на уровне чтения Excel файла, считаем что это 500-я ошибка
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }

        /// <summary>
        /// Метод по получению метаданных справочника "Версии данных".
        /// </summary>
        /// <returns></returns>
        [Route("api/DataVersions/GetMetadata")]
        [HttpGet]
        public ActionResult<DirectoryMetadata> GetMetadata()
        {
            try
            {
                return new DirectoryMetadata(new DataVersion());
            }
            catch (Exception ex)
            {
                // Отлавливается исключение на уровне чтения Excel файла, считаем что это 500-я ошибка
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Версии данных".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/DataVersions/Elements/{id}")]
        [HttpGet("{id}", Name = "GetDataVersion")]
        public ActionResult<DataVersion> Get(int id) // GET: api/DataVersion/Elements/1
        {
            try
            {
                Dictionary<int, DataVersion> dataVersionElements = DataLoader.GetDataOfDataVersionDirectory();

                DataVersion dataVersionElement = new DataVersion();

                if (dataVersionElements.TryGetValue(id, out dataVersionElement))
                {
                    return dataVersionElement;
                }
                else
                {
                    // Кидаем 404-ю, так как запрашиваемый объект не найден
                    return this.StatusCode(404);

                    // TODO: Запись в логи
                }
            }
            catch (Exception ex)
            {
                // Кидаем 500-ю ошибку, так как поймали исключение во время чтения данных
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }
    }
}
