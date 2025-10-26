# TaskPulse – Task / Product Management API

Учебный пет-проект для демонстрации навыков .NET Backend разработки.

## ✅ Технологии

- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL + Docker
- AutoMapper
- Swagger/OpenAPI
- Clean Architecture (Core / Infrastructure / Api)

## 🚀 Запуск проекта

Требуется установленный Docker Desktop.

```bash
docker compose up -d   # запуск PostgreSQL
dotnet ef database update -p TaskPulse.Infrastructure -s TaskPulse.Api
dotnet run --project TaskPulse.Api
