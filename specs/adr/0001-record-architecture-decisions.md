# 1. Zapisywanie decyzji architektonicznych (ADR)

## Status

Zaakceptowany

## Kontekst

Potrzebujemy ustandaryzowanego sposobu na dokumentowanie kluczowych decyzji dotyczących architektury i wyborów technologicznych w projekcie SongSpiration. Brakuje nam historii wyjaśniającej, *dlaczego* dokonano konkretnych wyborów technologicznych (np. Vue, .NET, AlphaTab), a nie tylko jak one działają. Ułatwi to wprowadzanie nowych osób do projektu oraz zapobiegnie ponownemu roztrząsaniu dawno zamkniętych tematów.

## Decyzja

Będziemy używać formatu Architecture Decision Records (ADR) zaproponowanego przez Michaela Nygarda. Dokumenty będą przechowywane w repozytorium w katalogu `specs/adr/` i numerowane sekwencyjnie.

## Konsekwencje

* **Pozytywne:** Mamy jedno źródło prawdy o decyzjach projektowych. Zrozumienie kontekstu historycznego będzie łatwiejsze.
* **Negatywne:** Wymaga to dyscypliny w zespole, aby aktualizować ADR-y w miarę ewolucji architektury systemu.
