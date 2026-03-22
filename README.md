# 🌳 Family Tree API

REST API для управления семейным древом, разработанное на ASP.NET Core 9.0.

## 🚀 Технологии

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Docker)
- FluentValidation
- xUnit + Moq + FluentAssertions
- Swagger/OpenAPI

## 📁 Архитектура

Проект использует Clean Architecture:
- **Domain** — сущности и бизнес-объекты
- **Application** — бизнес-логика и DTO
- **Infrastructure** — EF Core, БД, внешние сервисы
- **Api** — контроллеры и точка входа

## 🛠️ Запуск

```bash
# 1. Запуск базы данных
docker compose up -d

# 2. Применение миграций
dotnet ef database update

# 3. Запуск API
dotnet run --project src/FamilyTree.Api

# 4. Открыть Swagger
# https://localhost:7001