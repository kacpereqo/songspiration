# 4. Użycie MS SQL Server oraz Entity Framework Core

## Status

Zaakceptowany

## Kontekst

Potrzebujemy relacyjnej bazy danych do przechowywania danych użytkowników, pinów, gatunków muzycznych oraz polubień z racji tego, że dane te są mocno ze sobą powiązane (np. relacje many-to-many między pinami a gatunkami). Dodatkowo, operujemy w ekosystemie .NET dla backendu.

## Decyzja

Zdecydowaliśmy się użyć SQLite oraz Entity Framework (EF) Core jako narzędzia ORM (Object-Relational Mapping).

## Konsekwencje

* **Pozytywne:** Szybki rozwój dzięki EF Core (Code First, łatwe migracje). SQLite dobrze integruje się z aplikacjami ASP.NET Core i zapewnia stabilność oraz zgodność z właściwościami ACID dla naszych relacji.
* **Negatywne:** Narzut wydajnościowy w przypadku bardzo złożonych zapytań generowanych przez ORM, co może wymagać w przyszłości pisania zapytań surowych (Raw SQL).
