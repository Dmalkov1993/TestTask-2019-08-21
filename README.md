# TestTask-2019-08-21

Результат выполнения тестового задания.

Кратко о задании: нужно было реализовать два **REST Web Api .NET Core** сервиса - сервис справочников и сервис отчетов.

Исходный текст задания [в .docx документе (загрузить).](https://github.com/Dmalkov1993/TestTask-2019-08-21/raw/master/%D0%97%D0%B0%D0%B4%D0%B0%D0%BD%D0%B8%D0%B5%20TestTask-2019-08-21.docx)

Сервис справочников - умеет предоставлять данные 
и метаданные для двух справочников: «Объекты строительства» и «Версии данных».

Справочники реализовал с помощью Excel файлов.

Данные забираются из Excel файлов посредством библиотеки ClosedXML.

Сервис отчётов обращается к сервису справочников и строит отчёт, в зависимости от настроек.
Всего добавлено три типа настроек отчётов в [appSettings.json](https://github.com/Dmalkov1993/TestTask-2019-08-21/blob/master/ReportService/ReportService/appsettings.json), секция "ReportSettings":

- Настройка с ConfigurationID = 1 это отчёт согласно заданию;
- Настройка с ConfigurationID = 2 (справочники поменяны местами относительно Настройки с ID = 1);
- Настройка с ConfigurationID = 3 (добавил побольше атрибутов справочников в отчёт).

**Как запустить и использовать:**

1) Запустить солюшен DirectoryService\DirectoryService.sln прямо из под студии;
- 1.1) При первом запуске нужно нажать "Yes" на вопрос о доверии к SSL сертификату;
- 1.2) В окне запроса на установку сертификата для localhost нажать "Да".

2) Откроется страница api\AppStart, выведется информация о считанных объектах справочников
3) Получение данных производится следующим образом.
- 3.1) Данные справочника "Объекты строительства":
  - Получить все данные: https://localhost:44366/api/ConstructionObjects/GetAllDirectory
  - Получить один из элементов: https://localhost:44366/api/ConstructionObjects/Elements/[id], где **id** - от 1 до 4;
  - **Запрос метаданных** о справочнике: https://localhost:44366/api/ConstructionObjects/GetMetadata

- 3.2) Данные справочника "Версии данных":
  - Получить все данные: https://localhost:44366/api/DataVersions/GetAllDirectory
  - Получить один из элементов: https://localhost:44366/api/DataVersions/Elements/[id], где **id** - от 1 до 3;
  - **Запрос метаданных** о справочнике: https://localhost:44366/api/DataVersions/GetMetadata
  
4) Открыть солюшен ReportService\ReportService.sln
5) Нажать "Запустить" (прямо из под студии).
   Запустится страница https://localhost:44348/api/Report/1, где 1 - ИД конфигурации отчёта из AppSettings.json.
   
В случае успеха, в открытом окне браузера будет показан отчёт.
   
**О конфигурации отчётов.**
В [appSettings.json](https://github.com/Dmalkov1993/TestTask-2019-08-21/blob/master/ReportService/ReportService/appsettings.json) указаны три конфигурации отчётов, с номерами 1 2 и 3. 
Соответственно, они доступны по следующим URL:
- https://localhost:44348/api/Report/1 - (отчёт согласно заданию)
- https://localhost:44348/api/Report/2
- https://localhost:44348/api/Report/3

В солюшене сервиса отчётов есть два билдера отчётов - MockReportBuilder и TestTaskReportBuilder.
Они подтягиваются через DI, в зависимости от того, какой будет зарегистрирован в Startup-е сервиса отчётов.