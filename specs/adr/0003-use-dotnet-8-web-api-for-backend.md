# 3. Wybór C# i ASP.NET Core (.NET 8) do budowy API

## Status

Zaakceptowany

## Kontekst

Wymagamy solidnego i skalowalnego backendu, który dostarczy REST API dla frontendu aplikacji SongSpiration. Backend musi obsługiwać uwierzytelnianie, zarządzać logiką biznesową kolekcjonowania pinów, autoryzacją użytkowników i ewentualnym przetwarzaniem plików (w tym plików muzycznych).

## Decyzja

Wybraliśmy technologię C# z frameworkiem ASP.NET Core w najnowszej stabilnej wersji (.NET 8) dla warstwy backendowej.

## Konsekwencje

* **Pozytywne:** Silne typowanie, świetna wydajność oraz ustandaryzowana architektura (wstrzykiwanie zależności z pudełka, middlewares). Bogaty ekosystem i narzędzia deweloperskie (Visual Studio/Rider).
* **Negatywne:** Należy utrzymać odpowiednią architekturę aplikacji (np. podział na DAL, BLL, API - co widać w projekcie), by w pełni wykorzystać potencjał i uniknąć długu technologicznego.
