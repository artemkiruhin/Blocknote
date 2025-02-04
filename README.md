# 📝 BLOCKNOTE

## 📌 Описание

Blocknote — это кроссплатформенное приложение для создания, редактирования и обмена заметками. Оно поддерживает синхронизацию между устройствами через авторизацию, а также экспорт в различные форматы, включая **Docx, MD, HTML и JSON**.

## 🚀 Возможности

- ✍️ **Markdown-редактор** с возможностью предпросмотра.
- 🔄 **Синхронизация** заметок между устройствами.
- 👥 **Совместный просмотр** заметок с другими пользователями.
- 📤 **Экспорт** заметок в **Docx, MD, HTML, JSON**.
- 🔐 **Авторизация** для безопасного доступа к заметкам.
- 🎨 **Гибкий интерфейс** благодаря MAUI и Blazor Hybrid.
- 📚 **История изменений** заметок и возможность восстановления предыдущих версий.
- 🧑‍💻 **Поддержка нескольких пользователей** для работы в команде и обмена заметками.

## 🛠️ Технологии

- **.NET MAUI Blazor Hybrid** — основа для кроссплатформенной разработки.
- **Entity Framework Core** — работа с базой данных.
- **PostgreSQL** — основная СУБД для хранения данных.
- **JWT (JSON Web Tokens)** — для авторизации и безопасности.
- **Blazor** — для интерактивных веб-компонентов.

## 📥 Установка

1. Установите **.NET SDK** (.NET 9).
    
2. Клонируйте репозиторий:
    
    ```sh
    git clone https://github.com/your-repo/maui-blazor-notes.git
    cd maui-blazor-notes
    ```
    
3. Восстановите зависимости:
    
    ```sh
    dotnet restore
    ```
    
4. Добавьте `appsettings.json` файл в директорию `Blocknote.MAUIFrontend`:
    

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

5. Сделайте сборку проектов `Blocknote.Core` и `Blocknote.Api`:

```sh
    dotnet build Blocknote.Core
    dotnet build Blocknote.Api
```

6. Создайте миграцию:

```sh
    dotnet ef migrations add Init -p Blocknote.Core -s Blocknote.Api --context AppDbContext
```

7. Обновите базу данных:

```sh
    dotnet ef database update -p Blocknote.Core
```

8. Запустите приложение:
    
    ```sh
    dotnet build
    dotnet run
    ```
    

## 🛠️ Настройка

- Если вы хотите использовать другую СУБД вместо PostgreSQL, установите соответствующий NuGet-пакет и измените строку подключения в `appsettings.json` (в директории `Blocknote.MAUIFrontend`) в разделе **ConnectionStrings**:

```json
"ConnectionStrings": {
    "Database": "your_another_sql_connection_string"
}
```

## 📄 Лицензия

Этот проект распространяется под лицензией **MIT**.

## 👥 Контакты

- **Автор:** Kiruhin Artem
- **Email:** [angxlprod@gmail.com](mailto:angxlprod@gmail.com)
- **Telegram:** [@glamfuneral](https://t.me/glamfuneral)

---

💡 **Приглашаем к сотрудничеству!** Если у вас есть идеи по улучшению Blocknote, создавайте **issues** или **pull requests**. 🎉
