# FlightsLoggerAPI

Тестовый проект для TUI Fun & Sun


У нас приходят данные об изменении времени вылета рейса пассажира.
Они приходят в следующем формате:
```
{
    DepartureDate: DateTime,
    flightNumber: string
}
```
Нужно сделать 2 микросервиса, один web API, при вызове кладет данные об изменении времени вылета рейса в кафку, а второй читает из Кафки и кладет в SQL базу

Сделать на .net c#