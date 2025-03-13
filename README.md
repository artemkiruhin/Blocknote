# 📝 BLOCKNOTE

## 📌 Описание

Blocknote — это веб-приложение для создания, редактирования и обмена заметками. Оно поддерживает синхронизацию между устройствами через авторизацию, а также экспорт в различные форматы, включая **Docx, MD, HTML и JSON**.

## 🚀 Возможности

- ✍️ **Markdown-редактор** с возможностью предпросмотра.
- 🔄 **Синхронизация** заметок между устройствами.
- 👥 **Совместный просмотр** заметок с другими пользователями.
- 📤 **Экспорт** заметок в **Docx, MD, HTML, JSON**.
- 🔐 **Авторизация** для безопасного доступа к заметкам.
- 🎨 **Гибкий интерфейс** благодаря React.
- 📚 **История изменений** заметок и возможность восстановления предыдущих версий.
- 🧑‍💻 **Поддержка нескольких пользователей** для работы в команде и обмена заметками.

## 🛠️ Технологии

- **C# + .NET** — основа разработки.
- **Entity Framework Core** — работа с базой данных.
- **PostgreSQL** — основная СУБД для хранения данных.
- **JWT (JSON Web Tokens)** — для авторизации и безопасности.
- **React** — для интерактивных веб-компонентов.

## 📥 Установка
1. Установите **.NET SDK** (.NET 9).
    
2. Клонируйте репозиторий:
    
    ```sh
    git clone https://github.com/artemkiruhin/Blocknote.git
    cd Blocknote
    ```
## 🚀 Настройка и запуск
### Backend
1. Восстановите зависимости:
    
    ```sh
    dotnet restore
    ```
    
2. Добавьте `appsettings.json` файл в директорию `Blocknote.Api`:
    

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "ConnectionStrings": {
        "Database": "Postgresql_connection_string_here"
    },
    "jwt": {
        "audience": "your_audience",
        "issuer": "your_issuer",
        "key": "your_super_secret_key",
        "expires": 100
    }
}
```

3. Сделайте сборку проектов `Blocknote.Core` и `Blocknote.Api`:

```sh
    dotnet build Blocknote.Core
    dotnet build Blocknote.Api
```

4. Создайте миграцию:

```sh
    dotnet ef migrations add Init -p Blocknote.Core -s Blocknote.Api --context AppDbContext
```

5. Обновите базу данных:

```sh
    dotnet ef database update -p Blocknote.Core
```

### Fronend
1. Перейдите в директорию **frontend**:
```sh
    cd frontend
```

2. Установите зависимости
```sh
    npm install
```

### Запуск

1. Запустите ***сервер***:
    
    ```sh
    cd Blocknote.Api
    dotnet run
    ```
2. Запустите ***клиент***:
    
    ```sh
    cd frontend
    npm run start
    ``` 

## 📄 Лицензия

Этот проект распространяется под лицензией **MIT**.

## 👥 Контакты

- **Автор:** Kiruhin Artem
- **Email:** [angxlprod@gmail.com](mailto:angxlprod@gmail.com)
- **Telegram:** [@glamfuneral](https://t.me/glamfuneral)

---

💡 **Приглашаем к сотрудничеству!** Если у вас есть идеи по улучшению Blocknote, создавайте **issues** или **pull requests**. 🎉
