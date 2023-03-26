# TicTacToe.WebApi

## Описание проекта
Этот проект представляет собой ASP.NET Core Web API приложение, которое предоставляет REST API для игры в крестики нолики 3x3 для двух игроков. Приложение использует паттерн Repository-Service-Controller для управления игровыми данными и логикой игры.

## Используемые технологии
- .NET 6
- Entity Framework Core 7.0.4
- База данных MS SQL

## Запуск приложения
Для запуска приложения необходимо выполнить следующие шаги:

1. Установить .NET 6 SDK на свой компьютер.
2. Склонировать репозиторий: `git clone https://github.com/1yoga/TicTacToe.WebApi.git`
3. Открыть проект в Visual Studio.
4. Выполнить сборку проекта.
5. Установить ConnectionString в appsettings.json файле, указав свои параметры подключения к базе данных MS SQL.
6. Открыть консоль диспетчера пакетов и выполнить команду: `Update-Database`
  Это создаст базу данных и применит миграции, необходимые для работы приложения.
7. Запустить приложение в Visual Studio.


Также Web Api выложен на хостинг SmarterASP.NET и доступен по адруесу http://antonkazakevich-001-site1.etempurl.com/swagger/index.html

## Описнаие игры
<ol><li><p>Создание игрока 1:</p><ul><li>Клиент отправляет POST-запрос на эндпоинт <code>/api/player</code> с параметром <code>playerName</code>, содержащим имя игрока 1.</li><li>Сервер создает новый объект типа <code>Player</code> с указанным именем и присваивает ему уникальный идентификатор.</li><li>Сервер возвращает клиенту созданный объект игрока 1 со статусом 200 OK.</li></ul></li><li><p>Создание игрока 2:</p><ul><li>Клиент отправляет POST-запрос на эндпоинт <code>/api/player</code> с параметром <code>playerName</code>, содержащим имя игрока 2.</li><li>Сервер создает новый объект типа <code>Player</code> с указанным именем и присваивает ему уникальный идентификатор.</li><li>Сервер возвращает клиенту созданный объект игрока 2 со статусом 200 OK.</li></ul></li><li><p>Создание игры:</p><ul><li>Клиент отправляет POST-запрос на эндпоинт <code>/api/game</code> с параметрами <code>firstPlayerId</code> и <code>secondPlayerId</code>, содержащими идентификаторы игроков 1 и 2 соответственно.</li><li>Сервер создает новый объект типа <code>Game</code> с указанными идентификаторами игроков и присваивает игре уникальный идентификатор.</li><li>Сервер возвращает клиенту созданный объект игры со статусом 200 OK.</li></ul></li><li><p>Выполнение хода:</p><ul><li>Клиент отправляет POST-запрос на эндпоинт <code>/api/Game/{gameId}/createMove</code> с параметрами <code>gameId</code>, <code>playerId</code>, и <code>cell</code>.</li><li>Сервер проверяет, что указанный идентификатор игры существует и находится в статусе "в процессе игры".</li><li>Сервер проверяет, что указанный идентификатор игрока соответствует одному из игроков в указанной игре.</li><li>Сервер проверяет, что сейчас очередь ходить указанного игрока.</li><li>Сервер проверяет, что указанная ячейка свободна для хода.</li><li>Сервер сохраняет информацию о ходе в базу данных и обновляет состояние игры.</li><li>Сервер возвращает клиенту обновленный объект игры со статусом 200 OK.</li></ul></li><li><p>Завершение игры:</p><ul><li>Когда игра заканчивается, сервер обновляет состояние игры и сохраняет информацию о победителе или ничьей.</li><li>Сервер возвращает клиенту объект игры со статусом 200 OK и указанием результата игры (ничья, победа первого игрока, победа второго игрока).</li></ul></li></ol>

## Описание API 

### Player

<table>
  <thead>
    <tr>
      <th>HTTP метод</th>
      <th>Эндпоинт</th>
      <th>Описание</th>
      <th>Тело запроса (JSON)</th>
      <th>Тело ответа (JSON)</th>
      <th>Возможные ошибки</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>GET</td>
      <td>/api/player&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
      <td>Получить всех игроков</td>
      <td>Нет</td>
      <td>Массив&nbsp;объектов Player</td>
      <td>404 Not Found</td>
    </tr>
    <tr>
      <td>GET</td>
      <td>/api/player/{id}</td>
      <td>Получить игрока по ID</td>
      <td>Нет</td>
      <td>Объект Player</td>
      <td>404 Not Found</td>
    </tr>
    <tr>
      <td>POST</td>
      <td>/api/player</td>
      <td>Создать&nbsp;нового игрока</td>
      <td>
        {
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;"playerName":&nbsp;"string"
        <br>
        }
        </td>
      <td>Объект Player</td>
      <td></td>
    </tr>
    <tr>
      <td>PUT</td>
      <td>/api/player/{id}</td>
      <td>Обновить данные игрока</td>
      <td>Объект Player</td>
      <td>Обновленный объект Player</td>
      <td>404&nbsp;Not&nbsp;Found;<br>400&nbsp;Bad&nbsp;Request</td>
    </tr>
    <tr>
      <td>DELETE</td>
      <td>/api/player/{id}</td>
      <td>Удалить игрока</td>
      <td>Нет</td>
      <td>Нет</td>
      <td>404 Not Found</td>
    </tr>
  </tbody>
</table>

### Game

<table>
  <thead>
    <tr>
      <th>HTTP метод</th>
      <th>Эндпоинт</th>
      <th>Описание</th>
      <th>Тело запроса (JSON)</th>
      <th>Тело ответа (JSON)</th>
      <th>Возможные ошибки</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>GET</td>
      <td>/api/Game</td>
      <td>Получить список всех игр</td>
      <td>Нет</td>
      <td>Массив&nbsp;объектов Game</td>
      <td>404 Not Found</td>
    </tr>
    <tr>
      <td>GET</td>
      <td>/api/Game/{id}</td>
      <td>Получить игру по её ID</td>
      <td>Нет</td>
      <td>Объект Game</td>
      <td>404 Not Found</td>
    </tr>
    <tr>
      <td>POST</td>
      <td>/api/Game</td>
      <td>Создать новую игру</td>
      <td>
        {
            <br>
            &nbsp;&nbsp;&nbsp;&nbsp;"firstPlayerId": 1,
            &nbsp;&nbsp;&nbsp;&nbsp;"secondPlayerId": 2
            <br>
        }
      </td>
      <td>Объект Game</td>
      <td>404 Not&nbsp;Found;<br>400 Bad&nbsp;Request</td>
    </tr>
    <tr>
      <td>POST</td>
      <td>/api/Game/{gameId}/createMove</td>
      <td>Сделать ход в игре</td>
      <td>
        {
            <br>
            &nbsp;&nbsp;&nbsp;&nbsp;"playerId": 1,<br>
            &nbsp;&nbsp;&nbsp;&nbsp;"cell": 5
            <br>
        }
      </td>
      <td>Объект Game</td>
      <td>404&nbsp;Not&nbsp;Found;<br>400&nbsp;Bad&nbsp;Request</td>
    </tr>
    <tr>
      <td>DELETE</td>
      <td>/api/Game/{id}</td>
      <td>Удалить игру</td>
      <td>Нет</td>
      <td>Нет</td>
      <td>404 Not Found</td>
    </tr>
  </tbody>
</table>
