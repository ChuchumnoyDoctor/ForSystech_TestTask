# ForSystech_TestTask

За несколько вечеров было сделано только то, что относится непосредственно к ТЗ. 
Вспомогательные классы, методы и функии - движок и базис существующего проекта с текущего места работы. Он также написан собственноручно, но в гораздо ранее время. 
Упрощает реализацию, наследование и масштабирование Текущего Тестового Задания. 
Все это собственные наработки, не вижу в этом ничего "дозволенного" или упрощенного, в плане корректности выполнения ТЗ.
С согласия текущего работодателя, выкладывается в открытый доступ вместе с Тестовым Заданием.

Решение представлено тремя проектами:
1) Client - графический интерфейс, разработанный на WinForms
2) Server - консольное приложение, выполняет роль обработчика данных от клиента до БД.
3) dll - собственная библиотека. Включает в себя код подключения по TCP/IP, протоколы обработки типизированны данных, структуры классов и т.д.

А также DataBase on SQLite - локальная БДшка.

Инструкция по применению:
1) Запускаем сервер: запуск через .exe откомпилированного проекта или сам проект - как удобно. Все пути подключения настроены локально относительно друг друга в репозитории.
1.1) Подключается к БД, обрабатывает и хранит в памяти.
2) Запускаем клиент. Подключается к серверу автоматически, когда тот доступен, посредством TCP/IP and Sockets.
2.1) Представлен в виде формы авторизации и главной формы с вкладками, где последние - таблицы, являющиеся конечным инструментарием пользователя. Таблицы снабжены контекстным меню.
2.2) Код обработки не предполагает вывод ошибки, но, в большинстве случаев, учитывает и не позволяет исполнять не корректные входные данные. Поведение подобно транзакциям.
2.3) С каждым внесенным изменением пользователя, отправляется структура измененных данных на сервер, где проходит конечную обработку, их утверждение с последующим экспортом в Базу Данных.
3) dll является связующим звеном между сервером и клиентом. Для работы сервера и клиента со статичной типизацией, а не динамической или анонимной.
4) БД представлена минимальным количеством таблиц, полей и строк. Выполняет хранение минимально необходимых данных.
