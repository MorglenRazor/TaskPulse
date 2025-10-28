# TaskPulse – .NET 8 Microservice API Example

Учебный pet-проект для демонстрации навыков **Backend разработки на .NET** с продакшн-подходом.
Сервис реализует управление товарами (CRUD): название, цена, остаток.
Добавлена авторизация через JWT.

---

## 🚀 Технологический стек

| Технология | Применение |
|-----------|------------|
| **ASP.NET Core 8 Minimal API** | Backend REST API |
| **Entity Framework Core + PostgreSQL** | ORM и основная база данных |
| **JWT Authentication** | Авторизация пользователей |
| **ASP.NET Core Identity (lite)** | Хеширование паролей |
| **Docker + docker-compose** | Контейнеризация API и БД |
| **AutoMapper** | Маппинг DTO ↔ Entity |
| **Swagger/OpenAPI** | Документация + кнопка Authorize |
| **xUnit + FluentAssertions** | Unit-тестирование |
| **GitHub Actions CI** | Автоматическая сборка и тесты |

---

## 📦 Возможности

✔ CRUD для товаров  
✔ DTO + валидация входных моделей  
✔ EF Core миграции + сиды данных  
✔ JWT Login / Register  
✔ Защита эндпоинтов токеном  
✔ Контейнеризация всей инфраструктуры (API + PostgreSQL)  
✔ CI: build + тесты при push/pull request  

---

## 🛠 Запуск проекта

Необходимо установить **Docker Desktop** и .NET SDK 8.0+

```bash
docker compose up -d --build
``` 

