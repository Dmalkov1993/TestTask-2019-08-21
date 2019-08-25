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
        public ActionResult<string> Get()
        {
            try
            {
                // Заодно проверим чтение справочников из псевдо-БД
                var dataVersionDirectory = DataLoader.GetDataOfDataVersionDirectory();
                var objectsOfBuildingDirectory = DataLoader.GetDataOfConstructionObjectsDirectory();

                string response =
                $"App start successfully! Total readed objects: Data versions: {dataVersionDirectory.Count} Construction objects: {objectsOfBuildingDirectory.Count}" +
                    "\n\nTry get Construction objects: " +
                    "https://localhost:44366/api/ConstructionObjects/Elements/1 " +
                    "\nor " +
                    "https://localhost:44366/api/ConstructionObjects/GetAllDirectory to get all Construction objects" +
                    "\n\nTry get Data Versions: " +
                    "https://localhost:44366/api/DataVersions/Elements/1 " +
                    "\nor " +
                    "https://localhost:44366/api/DataVersions/GetAllDirectory to get all Data Version objects" +

                    "\n\n\nGet metadata of directories:" +
                    "\n https://localhost:44366/api/ConstructionObjects/GetMetadata" +
                    "\n https://localhost:44366/api/DataVersions/GetMetadata";

                // Если не упали - говорим Ура.
                return response; //this.StatusCode(200, response);
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
