namespace DirectoryService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using ClosedXML.Excel;
    using DirectoryService.DataLoaders;
    using DirectoryService.Models;
    using Microsoft.AspNetCore.Http;
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
        public IEnumerable<ОбъектСтроительства> Get() // GET: api/Construction
        {
            return DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства().Values;
        }

        /// <summary>
        /// Метод по получению одной записи из справочника "Объекты строительства".
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public ОбъектСтроительства Get(int id) // GET: api/Construction/1
        {
            return DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства()[id];
        }
    }
}
