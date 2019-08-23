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
        public ActionResult Get()
        {
            try
            {
                // Заодно проверим чтение справочников из псевдо-БД
                var версииДанных = DataLoader.ПолучитьДанныеСправочникаВерсииДанных();
                var объектыСтроительства = DataLoader.ПолучитьДанныеСправочникаОбъектыСтроительства();

                string response =
                $"App start successfully! Total readed objects: Data versions: {версииДанных.Count} Construction objects: {объектыСтроительства.Count}" +
                    "\n\nTry get Construction objects: https://localhost:44366/api/Construction/3 " +
                    "\nor https://localhost:44366/api/Construction to get all Construction objects" +
                    "\n\nTry get Data Versions: https://localhost:44366/api/DataVersion/3 " +
                    "\nor https://localhost:44366/api/DataVersion to get all Data Version objects" +
                    "\n\n\nGet metadata of directories:" +
                    "\n https://localhost:44366/api/Construction?getMeta=true" +
                    "\n https://localhost:44366/api/DataVersion?getMeta=true";

                // Если не упали - говорим Ура.
                return this.StatusCode(200, response);
            }
            catch
            {
                // return "Произошла ошибка при чтении справочников.";

                // Отлавливается исключение на уровне чтения Excel файла, считаем что это 500-я ошибка
                return this.StatusCode(500);
            }
        }
    }
}
