# 6. Użycie biblioteki AlphaTab do renderowania tabulatur

## Status

Zaakceptowany

## Kontekst

Kluczową funkcjonalnością SongSpiration jest wyświetlanie nut i tabulatur z plików `.gp` i `.gp5` bezpośrednio w przeglądarce, wraz z możliwością ich odsłuchania, bez konieczności instalowania przez użytkownika zewnętrznych programów.

## Decyzja

Zdecydowaliśmy się wykorzystać bibliotekę JavaScript AlphaTab.

## Konsekwencje

* **Pozytywne:** AlphaTab to natywne przeglądarkowe rozwiązanie zdolne do renderowania plików Guitar Pro i generowania dźwięku za pomocą własnego syntezatora. Pozwala na zbudowanie bogatego Playera Webowego z zaawansowaną integracją (np. wskazywanie aktualnie granego taktu na frontendzie).
* **Negatywne:** Złożoność biblioteki wymaga jej starannego obsłużenia. Może wystąpić duże zużycie zasobów po stronie przeglądarki klienta przy bardzo dużych plikach partytur.
