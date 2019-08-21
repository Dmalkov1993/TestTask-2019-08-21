using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryService.DataLoaders;
using DirectoryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    [Route("api/DataVersion")]
    [ApiController]
    public class DataVersionController : ControllerBase
    {
        /// <summary>
        /// Метод по получению всех данных из справочника "Объекты строительства".
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ВерсияДанных> Get() // GET: api/DataVersion
        {
            return DataLoader.ПолучитьДанныеСправочникаВерсииДанных().Values;
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Объекты строительства".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public ВерсияДанных Get(int id) // GET: api/DataVersion/1
        {
            return DataLoader.ПолучитьДанныеСправочникаВерсииДанных()[id];
        }
    }
}
