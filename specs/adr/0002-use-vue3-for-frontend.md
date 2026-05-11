# 2. Wybór Vue 3 na framework frontendowy

## Status

Zaakceptowany

## Kontekst

Projekt SongSpiration wymaga stworzenia dynamicznej, responsywnej aplikacji webowej typu SPA (Single Page Application). Interfejs użytkownika będzie posiadał interaktywne elementy takie jak zaawansowane filtrowanie, renderowanie nut (AlphaTab) i system polubień, co wymaga wydajnego systemu reaktywności.

## Decyzja

Wybraliśmy Vue 3 (z Composition API) jako główny framework frontendowy.
Rozważaliśmy również React.js oraz Angular. Vue 3 został wybrany ze względu na:
* Wbudowany system reaktywności, który jest wydajny i łatwy w użyciu przy aktualizacji stanów (np. odtwarzacza audio).
* Czystą składnię i dobrą dokumentację, przyspieszającą development.
* Wsparcie dla TypeScript (poprzez Vite i tsconfig).

## Konsekwencje

* **Pozytywne:** Szybkie tempo powstawania UI. Wydajność działania aplikacji.
* **Negatywne:** Ekosystem bibliotek dla Vue jest nieco mniejszy niż w przypadku React, co może wymagać własnych implementacji niektórych bardziej specyficznych komponentów.
