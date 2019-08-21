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
        public IEnumerable<ВерсияДанных> Get() // GET: api/DataVersion
        {
            return DataLoader.ПолучитьДанныеСправочникаВерсииДанных().Values;
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Версии данных".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetDataVersion")]
        public ВерсияДанных Get(int id) // GET: api/DataVersion/1
        {
            return DataLoader.ПолучитьДанныеСправочникаВерсииДанных()[id];
        }
    }
}
