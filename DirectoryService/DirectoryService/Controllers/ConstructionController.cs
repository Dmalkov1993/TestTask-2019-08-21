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
        public JsonResult Get() // GET: api/Construction
        {
            try
            {
                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства().Values);
            }
            catch
            {
                return new JsonResult(new EmptyResult());
            }
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Объекты строительства".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetConstruction")]
        public JsonResult Get(int id) // GET: api/Construction/1
        {
            try
            {
                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства()[id]);
            }
            catch
            {
                return new JsonResult(new EmptyResult());
            }
        }
    }
}
