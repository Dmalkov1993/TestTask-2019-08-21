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
            try
            {
                // Заодно проверим чтение справочников из псевдо-БД
                var версииДанных = DataLoader.ПолучитьДанныеСправочникаВерсииДанных();
                var объектыСтроительства = DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства();

                // Если не упали - говорим Ура.
                return
                    $"App start successfully! Total readed objects: Data versions: {версииДанных.Count} Construction objects: {объектыСтроительства.Count}" +
                      "\n\nTry get Construction objects: https://localhost:44366/api/Construction/3 " +
                      "\nor https://localhost:44366/api/Construction to get all Construction objects"+
                      "\n\nTry get Data Versions: https://localhost:44366/api/DataVersion/3 " +
                      "\nor https://localhost:44366/api/DataVersion to get all Data Version objects";
            }
            catch
            {
                return "Произошла ошибка при чтении справочников.";
            }
        }
    }
}
