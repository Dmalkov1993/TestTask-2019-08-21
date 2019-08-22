namespace DirectoryService.Controllers
{
    using System.Collections.Generic;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/DataVersion")]
    [ApiController]
    public class DataVersionController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Версии данных".
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get() // GET: api/DataVersion
        {
            try
            {
                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаВерсииДанных().Values);
            }
            catch
            {
                return new JsonResult(new EmptyResult());
            }
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Версии данных".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetDataVersion")]
        public JsonResult Get(int id) // GET: api/DataVersion/1
        {
            try
            {
                return new JsonResult(DataLoader.ПолучитьДанныеСправочникаВерсииДанных()[id]);
            }
            catch
            {
                return new JsonResult(new EmptyResult());
            }
        }
    }
}
