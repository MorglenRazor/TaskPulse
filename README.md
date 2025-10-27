# TaskPulse – .NET 8 Microservice API Example

Учебный pet-проект для демонстрации навыков **Backend разработки на .NET** с продакшен-подходом.
Сервис реализует управление товарами (CRUD): название, цена, остаток.

---

## 🚀 Технологический стек

| Технология | Применение |
|-----------|------------|
| **ASP.NET Core 8 Minimal API** | Backend REST API |
| **Entity Framework Core** | Работа с PostgreSQL |
| **PostgreSQL** | Основная база данных |
| **Docker + docker-compose** | Контейнеризация API и БД |
| **AutoMapper** | Маппинг DTO ↔ Entity |
| **Swagger/OpenAPI** | Документация API |
| **xUnit + FluentAssertions** | Unit тестирование |
| **GitHub Actions CI** | Автоматическая сборка |

---

## 📦 Возможности

✔ CRUD для товаров  
✔ DTO + валидация входных моделей  
✔ EF Core миграции  
✔ AutoMapper профиль  
✔ Контейнеризация всей инфраструктуры  

---

## 🛠 Запуск проекта

Необходимо установить **Docker Desktop** и .NET SDK 8.0+

```bash
docker compose up -d --build
