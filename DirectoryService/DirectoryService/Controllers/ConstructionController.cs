namespace DirectoryService.Controllers
{
    using System;
    using System.Collections.Generic;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Mvc;

    // [Route("api/Construction")] - можно указать и здесь аттрибут Route
    // или можно ещё в таком формате указывать роут: 
    // [Route("api/[controller]/[action]")]
    // или 
    // [Route("api/[controller]/[action]/{id}")]

    [ApiController]
    public class ConstructionObjectsController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Объекты строительства".
        /// </summary>
        /// <returns></returns>
        [Route("api/ConstructionObjects/GetAllDirectory")]
        [HttpGet]
        public ActionResult<Dictionary<int, ConstructionObject>.ValueCollection> Get()
        {
            // TODO: можно подобную конструкцию в контроллере DataVersionController вынести в метод базового контроллера
            try
            {
                return DataLoader.GetDataOfConstructionObjectsDirectory().Values;
            }
            catch (Exception ex)
            {
                // Отлавливается исключение на уровне чтения Excel файла, считаем что это 500-я ошибка
                return this.StatusCode(500);

                // TODO: Запись в логи
            }
        }

        /// <summary>
        /// Метод по получению метаданных из справочника "Объекты строительства".
        /// </summary>
        /// <returns></returns>
        [Route("api/ConstructionObjects/GetMetadata")]
        [HttpGet]
        public ActionResult<DirectoryMetadata> GetMetadata()
        {
            // TODO: можно подобную конструкцию в контроллере DataVersionController вынести в метод базового контроллера
            try
            {
                return new DirectoryMetadata(new ConstructionObject());
            }
            catch (Exception ex)
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
        [Route("api/ConstructionObjects/Elements/{id}")]
        [HttpGet("{id}", Name = "GetConstruction")]
        public ActionResult<ConstructionObject> Get(int id) // GET: api/Construction/1
        {
            try
            {
                Dictionary<int, ConstructionObject> objectsOfBuilding = DataLoader.GetDataOfConstructionObjectsDirectory();

                ConstructionObject objectOfBuilding = new ConstructionObject();

                if (objectsOfBuilding.TryGetValue(id, out objectOfBuilding))
                {
                    return objectOfBuilding;
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
