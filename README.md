# Advertising Platforms Service

Веб-сервис для управления рекламными площадками и поиска подходящих площадок для заданных локаций.

## Требования

- .NET 6.0 или выше
- Visual Studio 2022 или VS Code

## Запуск приложения

1. Клонируйте репозиторий:
```bash
git clone https://github.com/SosGutnov/AdvertisingPlatforms
cd AdPlatformService
```
2. Восстановите зависимости:
```bash
dotnet restore
```
3. Запустите приложение:
```bash
dotnet run
```
4. Приложение будет доступно по адресу: https://localhost:7000 или http://localhost:5000

# API Endpoints
1. Загрузка файла с рекламными площадками
POST /api/adplatform/upload

Загружает файл с рекламными площадками (полностью перезаписывает существующие данные).

Параметры:

file: Файл в формате текстового файла

Пример файла:
```text
Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd
```
2. Поиск рекламных площадок для локации
GET /api/adplatform/search?location={location}

Возвращает список рекламных площадок, действующих в указанной локации.

Параметры:

location: Путь локации (например, /ru/msk)

Пример ответа:
```json
["Газета уральских москвичей", "Яндекс.Директ"]
```

Запуск тестов
```bach
dotnet test
```
