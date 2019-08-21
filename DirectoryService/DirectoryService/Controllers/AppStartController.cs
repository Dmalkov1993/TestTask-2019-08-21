namespace DirectoryService.Controllers
{
    using DirectoryService.DataLoaders;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/AppStart")]
    [ApiController]
    public class AppStartController : ControllerBase
    {
        // GET: api/AppStart
        [HttpGet]
        public string Get()
        {
            // Заодно проверим чтение справочников из псевдо-БД
            var версииДанных = DataLoader.ПолучитьДанныеСправочникаВерсииДанных();
            var объектыСтроительства = DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства();

            // Если не упали - говорим Ура.
            return $"App start successfully! Total readed objects: Data versions: {версииДанных.Count} Construction objects: {объектыСтроительства.Count}";
        }
    }
}
