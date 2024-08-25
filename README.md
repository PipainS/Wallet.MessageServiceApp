# Message Exchange Service

## Описание

**Message Exchange Service** — это простой веб-сервис для обмена сообщениями, который состоит из следующих компонентов:
- **MessageService**: Серверная часть, обрабатывающая сообщения, записывающая их в базу данных и перенаправляющая их клиентам по WebSocket.
- **MessageClient1**: Клиент для отправки сообщений.
- **MessageClient2**: Клиент для отображения сообщений в реальном времени.
- **MessageClient3**: Клиент для просмотра истории сообщений за последние 10 минут.

## Ветки

В проекте есть две основные ветки:

- `withDocker` — содержит конфигурации Docker и Docker Compose для запуска проекта в контейнерах.
- `withoutDocker` — проект без Docker, который можно запускать напрямую через .NET SDK.

## Технологии

- .NET 8
- ASP.NET Core
- PostgreSQL
- WebSocket
- Docker (только в ветке `withDocker`)

## Требования

### Для ветки `withDocker`:
- Docker и Docker Compose

### Для ветки `withoutDocker`:
- .NET 8 SDK
- PostgreSQL (должен быть установлен и настроен локально)
