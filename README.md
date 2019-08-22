﻿# TestTask-2019-08-21

Тестовое задание.

Кратко о задании: нужно было реализовать два сервиса - сервис справочников и сервис отчетов.

Сервис справочников - умеет предоставлять данные 
и метаданные для двух справочников: «Объекты строительства» и «Версии данных».

Справочники реализовал с помощью Excel файлов.

Данные забираются из Excel файлов посредством библиотеки ClosedXML.


**Как запустить:**

1) Запустить солюшен DirectoryService\DirectoryService.sln прямо из под студии;
- 1.1) При первом запуске нужно нажать "Yes" на вопрос о доверии к SSL сертификату;
- 1.2) В окне запроса на установку сертификата для localhost нажать "Да".
   
2) Откроется страница api\AppStart, выведется информация о считанных объектах справочников
3) Получение данных производится следующим образом.
- 3.1) Данные справочника "Объекты строительства":
  - Получить все данные: https://localhost:44366/api/Construction
  - Получить один из элементов: https://localhost:44366/api/Construction/[id], 
												где **id** - от 1 до 4;
												
- 3.2) Данные справочника "Версии данных":
  - Получить все данные: https://localhost:44366/api/DataVersion
  - Получить один из элементов: https://localhost:44366/api/DataVersion/[id], 
												где **id** - от 1 до 3;
	   