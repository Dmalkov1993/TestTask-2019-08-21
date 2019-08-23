namespace DirectoryService.Controllers
{
    using System;
    using System.Collections.Generic;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/DataVersion")]
    [ApiController]
    public class DataVersionController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Версии данных" при getMeta = false.
        /// И возврату метаданных данного справочника при getMeta = true.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(bool getMeta = false) // GET: api/DataVersion or api/DataVersion?getMeta=true
        {
            try
            {
                if (getMeta)
                {
                    return new JsonResult(new МетаданныеСправочника(new ВерсияДанных()));
                }

                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаВерсииДанных().Values);
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
        [HttpGet("{id}", Name = "GetDataVersion")]
        public ActionResult Get(int id) // GET: api/DataVersion/1
        {
            try
            {
                Dictionary<int, ВерсияДанных> версииДанных = DataLoader.ПолучитьДанныеСправочникаВерсииДанных();

                ВерсияДанных версияДанных = new ВерсияДанных();

                if (версииДанных.TryGetValue(id, out версияДанных))
                {
                    // JsonResult сам вернёт 200
                    return new JsonResult(версияДанных);
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
