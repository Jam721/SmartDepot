# Во первых подписываемся на SachkovTech 
   - YouTube: https://www.youtube.com/@SachkovTech
   - Tg: [t.me/sachkov_blog](https://t.me/sachkov_blog)
   - Курс: [sachkov-dotnet.ru](https://sachkov-dotnet.ru/)
   - Twitch: [twitch.tv/sachkovtech](https://www.twitch.tv/sachkovtech)

# 📦 SmartDepot — Умный склад с характером и Петровичем в придачу

> **SmartDepot** — это API, где логистика встречает абсурд. Склады тут ворчат, предметы капризничают, а Петрович раздаёт советы, от которых плачут даже серверы.

--- 
## Мой тг
   - [@Bebekon1240](https://t.me/Bebekon1240)
---

# Функционал проекта можно проверить так
**docker-compose.yml**
```
services:
  api:
    image: bebekon/smartdepot-api:latest
    ports:
      - "8080:8080"
      - "8081:8081"
    depends_on:
      - db
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=db;Port=5432;Database=SmartDepot;User Id=postgres;Password=123;
    restart: unless-stopped

  db:
    image: postgres:15
    container_name: smartdepot-db
    environment:
      POSTGRES_DB: SmartDepot
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: 123
    ports:
      - "15432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    restart: unless-stopped

volumes:
  pgdata:

```
Потом перейти по адресу: http://localhost:8080/swagger/index.html

## 🔧 Эндпоинты API

---

### 📦 **Item**

| Метод  | URL                              | Описание                                  |
|--------|----------------------------------|-------------------------------------------|
| GET    | `/api/items/get_all`             | Все предметы (включая их "капризы")       |
| GET    | `/api/items/get_by_id/{id}`      | Предмет по ID. |
| GET    | `/api/items/mood`                | Коллекция настроений предметов |
| POST   | `/api/items/create`              | Создать предмет. |
| PUT    | `/api/items/update/{id}`         | Обновить данные. |
| DELETE | `/api/items/delete/{id}`         | Удалить предмет. |

**Форма добавления предмета**

![image](https://github.com/user-attachments/assets/315727b1-9d9e-4895-89ab-04295ad445bc)

---

### 🧔 **Petrovich**

| Метод  | URL                              | Описание                                  |
|--------|----------------------------------|-------------------------------------------|
| GET    | `/api/petrovich/advice`          | Получить жизненный совет от Петровича. Пример: *"Не храни арбузы в морозилке. Это не мороженое!"* |
| GET    | `/api/petrovich/status`          | Статус складов глазами Петровича: *"Склад №3 сегодня в депрессии. Дайте ему печеньку."* |
| POST   | `/api/petrovich/TestData`        | Заполнить базу тестовыми данными. |

---

### 🔄 **Transfer**

| Метод  | URL                              | Описание                                  |
|--------|----------------------------------|-------------------------------------------|
| GET    | `/api/transfers/get_all`         | Вся история перемещений. |
| POST   | `/api/transfers/create`          | Запустить перемещение. |

**Форма перемещения**

![image](https://github.com/user-attachments/assets/44ca7cf1-2902-4488-ab22-370e4a949433)

---

### 🏭 **Warehouse**

| Метод  | URL                              | Описание                                  |
|--------|----------------------------------|-------------------------------------------|
| GET    | `/api/warehouses/get_all`        | Все склады. |
| GET    | `/api/warehouses/get_by_id/{id}` | Склад по ID. |
| GET    | `/api/warehouses/get_items_by_id/{id}/items` | Предметы на складе. |
| POST   | `/api/warehouses/create`         | Создать склад. |
| PUT    | `/api/warehouses/update/{id}`    | Обновить склад. |
| DELETE | `/api/warehouses/delete/{id}`    | Удалить склад. |


**Форма добавление склада**

![image](https://github.com/user-attachments/assets/44f9f2ab-0cba-42d7-8105-3a7c4f8ec3f5)

---

### Фото сваггера
![image](https://github.com/user-attachments/assets/987011de-d26c-45a7-ab25-838c524f39cf)

# 🎯 Итоги хакатона: 48 часов кайфа

## ✅ Что получилось блестяще:
**Полный цикл API**  
   Все заявленные эндпоинты реализованы:
   - Управление складами, предметами, перемещениями
   - Система "капризов" предметов (настроение, условия хранения)
   - Интеграция с PostgreSQL + автоматические миграции
**Ответы петровича что-то с чем то, главное они каждый раз разные**

![image](https://github.com/user-attachments/assets/f1728f1a-f75a-4ea5-8048-3cd1db491f1e)


**Логика перемещений**  
   - Проверка условий хранения
     
---

## 🚧 Что осталось за кадром:
1. **Глубокая аналитика от Петровича**  
   - Планировалось добавить `/api/petrovich/analyze` с полным анализом данных и советами от Петровича, но... время кончилось.

2. **Сложные сценарии капризов**  
   - Предметы пока "грустят" только из-за температуры. Хотелось:
     - Ревность ("Почему соседнюю коробку переместили раньше?")
     - Лень ("Не хочу переезжать, тут удобно!")

---

## 🛠️ Технологии

- **.NET 9** + **PostgreSQL**.
- **Swagger**.
- **Entity Framework Core**.

---

## Структура
![image](https://github.com/user-attachments/assets/c7be15c4-ea5d-4506-94b5-600335f1e95a)


> **ВАЖНО**: Петрович всегда прав. Даже когда не прав. Особенно тогда.
