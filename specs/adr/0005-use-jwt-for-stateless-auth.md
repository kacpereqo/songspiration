# 5. Użycie JWT (JSON Web Tokens) do bezstanowej autoryzacji

## Status

Zaakceptowany

## Kontekst

Aplikacja musi obsługiwać bezpieczne logowanie, zróżnicowanie dostępu na role (User, Admin) oraz autoryzować żądania płynące ze SPA do serwera API. Chcemy, aby rozwiązanie to skalowało się dobrze i pozwalało na odseparowanie frontendu od backendu bez wymogu utrzymywania sesji po stronie serwera.

## Decyzja

Zdecydowaliśmy na bezstanową autoryzację opartą na tokenach JWT. Backend po poprawnym zalogowaniu wydaje token dostępu, który jest przekazywany w nagłówkach HTTP żądań. Token będzie krótkożyjący i współpracujący z systemem tokenów odświeżających (Refresh Tokens). Po stronie klienta token będzie przechowywany w `sessionStorage`.

## Konsekwencje

* **Pozytywne:** API pozostaje bezstanowe (stateless), co ułatwia skalowanie. Elastyczność w komunikacji klient-serwer.
* **Negatywne:** Przechowywanie w `sessionStorage` wiąże się z podatnością na ataki XSS, jednak zwalnia z problemów związanych z CSRF i użyciem ciasteczek. Konieczność implementacji logiki odświeżania tokenów.
