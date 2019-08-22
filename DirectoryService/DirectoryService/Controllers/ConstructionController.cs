namespace DirectoryService.Controllers
{
    using System.Collections.Generic;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/Construction")]
    [ApiController]
    public class ConstructionController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Объекты строительства".
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get() // GET: api/Construction
        {
            try
            {
                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства().Values);
            }
            catch
            {
                // Отлавливается исключение на уровне чтения Excel файла, считаем что это 500-я ошибка
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Объекты строительства".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetConstruction")]
        public ActionResult Get(int id) // GET: api/Construction/1
        {
            try
            {
                Dictionary<int, ОбъектСтроительства> объектыСтроительства = DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства();

                ОбъектСтроительства объектСтроительства = new ОбъектСтроительства();

                if (объектыСтроительства.TryGetValue(id, out объектСтроительства))
                {
                    // JsonResult сам вернёт 200
                    return new JsonResult(объектСтроительства);
                }
                else
                {
                    // Кидаем 404-ю, так как запрашиваемый объект не найден
                    return this.StatusCode(404);

                    // TODO: Запись в логи
                }
            }
            catch
            {
                // Кидаем 500-ю ошибку, так как поймали исключение во время чтения данных
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }
    }
}
