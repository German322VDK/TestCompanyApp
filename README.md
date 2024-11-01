# TestCompanyApp

## Запуск через ASP.Net core API + React

### Установка подключения клиента к API
В файле `test-company-app-client/src/config.js` установите значение `DefaultApiUrl`:
```javascript
DefaultApiUrl = "http://localhost:5001/api";
```

### Запуск клиента
В терминале выполните:
```powershell
cd test-company-app-client
npm start
```

### Запуск API
Запустите API из Visual Studio.

### Инициализация тестовыми данными
Для инициализации тестовых данных выполните следующую команду в PowerShell:
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5001/api/employees/settestdata?count=50" -Method Get
$response
```
или ввести в браузере:
[http://localhost:5001/api/employees/settestdata?count=50](http://localhost:5001/api/employees/settestdata?count=50)
---

## Запуск через Docker Compose

### Установка подключения клиента к API
В файле `test-company-app-client/src/config.js` установите значение `DefaultApiUrl`:
```javascript
DefaultApiUrl = "http://localhost:8080/api";
```

### Запуск через Docker Compose
Запустите `docker-compose` из Visual Studio.

- **Сайт:** [http://localhost:8080](http://localhost:8080)
- **Внешний адрес API:** [http://localhost:5002/api](http://localhost:5002/api)

### Инициализация тестовыми данными
Для инициализации тестовых данных выполните следующую команду в PowerShell:
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5002/api/employees/settestdata?count=50" -Method Get
$response
```
или ввести в браузере:
[http://localhost:5002/api/employees/settestdata?count=50](http://localhost:5002/api/employees/settestdata?count=50)
---

## Тестирование
Проект включает в себя модульные тесты для CommentStore, PostStore и TradStore. Чтобы запустить тесты, используйте GitHub Actions или выполните тесты локально с помощью следующей команды:
 ```bash
  dotnet test
 ```
[![Testing](https://github.com/German322VDK/TestCompanyApp/actions/workflows/test.yml/badge.svg)](https://github.com/German322VDK/TestCompanyApp/actions/workflows/test.yml)

---

## Документация
Чтобы ознакомится с документацией перейдите по [ссылке](https://german322vdk.github.io/TestCompanyApp/api/index.html)



