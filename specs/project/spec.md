# Specyfikacja Projektu: MusicPin (Wersja Rozszerzona)

## 1. Cel Projektu
**SongSpiration** to aplikacja webowa służąca do kolekcjonowania i przeglądania inspiracji muzycznych w formie wizualnych "pinów". Platforma dedykowana jest muzykom, umożliwiając im kategoryzację pomysłów, podgląd nut oraz współdzielenie plików tabulatur.

---

## 2. Stos Technologiczny
* **Frontend:** Vue 3
* **Backend:** C# / ASP.NET Core Web API (.NET 8)
* **Baza danych:** SQL Server + Entity Framework Core

---

## 3. Architektura Systemu i Funkcjonalności

### 3.1. System Użytkowników i Autoryzacja
* **Rejestracja:** Zakładanie nowego konta z wykorzystaniem adresu e-mail, nazwy użytkownika oraz hasła.
* **Logowanie:** Uwierzytelnianie przy pomocy e-maila i hasła. Sukces logowania nagradzany jest wygenerowaniem tokena JWT, który jest używany do autoryzacji w systemie.
* **Autoryzacja (JWT):** Zabezpieczenie API chronionych ścieżek nagłówkiem `Authorization: Bearer <token>`. Wykorzystanie `sessionStorage` do po stronie frontendu.
* **Role systemowe:** Podział użytkowników na zwykłych klientów oraz Administratorów, uwzględniając panel administracyjny (CRUD).

### 3.2. Zarządzanie Treścią (Pins & Tabs)
* **Upload:** Formularz przesyłania plików `.gp` wraz z metadanymi - tytuł, gatunki muzyczne
* **Automatyzacja:** Instrument zostanie wybrany samemu na podstawie przesłanego pliku `.gp`
* **Podgląd wizualny:** Przechowywanie grafik reprezentujących zapis nutowy dla każdego pinu.

### 3.3. System Filtrowania i Wyszukiwania
* **Filtrowanie po Instrumencie:** Szybki wybór kategorii: Gitara, Bas, Perkusja.
* **Filtrowanie po Gatunku:** Możliwość kategoryzowania pinów według gatunków muzycznych (np. Rock, Metal, Jazz, Blues).
* **Sortowanie** Sortowanie po popularności na podstawie własnego algorytmu, polubień lub według daty publikacji 

### 3.4. Interaktywny Podgląd (Web Player)
<!-- TODO 
    LENIWE PRZETWARZANIE OBRAZU - 
-->

* **Renderowanie Nut:** Integracja z biblioteką AlphaTab umożliwiającą wyświetlanie tabulatur bezpośrednio w przeglądarce
* **Odtwarzanie:** Możliwość odsłuchania zapisu MIDI wygenerowanego z pliku `.gp`.

### 3.5. Kolekcje i Social
* **Polubienie:** Funkcja "Serduszko" pozwalająca na polubienie pina i dodanie go do zakładki "polubione"

---

## 4. Zadania Warstwy Logiki Biznesowej
* **Service Layer:** Implementacja serwisów `UserService`, `PinService` oraz `CollectionService`.
* **Logika Filtrowania:** Budowanie dynamicznych zapytań do bazy danych na podstawie wybranych filtrów instrumentów i gatunków.
* **Unit Testing:** * Testy logiki rejestracji
    * Testy mechanizmu filtrowania 
    * Testy integracyjne zapisu plików na serwerze.
---

## 5. Model Danych

### 5.1. Wysokopoziomowy opis tabel

| Tabela | W skrócie — co zawiera |
|---|---|
| `User` | Dane konta: email, hash hasła, profil użytkownika (display_name, avatar), role oraz metadane (created_at, last_login, is_email_verified). |
| `AuthToken/Session` | Tokeny sesyjne i odświeżające: hash tokenu, typ (access/refresh), data wygaśnięcia, revokacja, odniesienie do użytkownika. |
| `Pin` | Metadane pina: tytuł, opis, właściciel, lokalizacja lub blob pliku `.gp`, metadane pliku (filename, mime_type, size, checksum), widoczność, znaczniki czasowe. |
| `Genre` | Lista gatunków muzycznych (nazwa, slug). |
| `PinGenre` | Powiązania many-to-many między `Pin` a `Genre` (mapowanie pin↔gatunek). |
| `Like` | Rejestracja polubień: kto polubił który pin oraz data; unikatowy klucz (`user_id`,`pin_id`). |


---

## 6. Interfejs Użytkownika (UI)
### 6.1. Widoki publiczne i nawigacja

- **Układ aplikacji:** górny pasek nawigacji z: logo (powrót do feedu), wyszukiwarka (tytuł/autor), skrót do `Upload`, skrót do profilu (po zalogowaniu) oraz przyciski `Zaloguj` / `Zarejestruj` (dla niezalogowanych).

- **Strona główna / widok `pins` (feed):** responsywny grid z infinite scroll.

### 6.2. Karta pina i szczegóły

- **Karta pina (w gridzie):**
  - miniatura (wyrenderowany obraz tabulatury), `title`, autor (link do profilu), metadane (gatunki, instrument, czas utworzenia)
  - akcje: `play/pause` (podgląd MIDI), `like` (licznik polubień), `save` (dodaj do kolekcji), `share`, `open details`

- **Interakcje karty pina:**
  - kliknięcie miniatury / `open details` otwiera modal / stronę szczegółów z: pełnym podglądem (AlphaTab), odtwarzaczem MIDI, pełnymi metadanymi, listą gatunków, akcjami `download` (jeśli dozwolone) oraz `edit`/`delete` (tylko właściciel)
  - hover na karcie (desktop): szybkie kontrolki odtwarzania i ikona zapisu do kolekcji

- **Dostępność i stany:** skeleton loadery, komunikaty braku wyników, obsługa błędów odtwarzacza/podglądu (np. uszkodzony plik), informacja o pinach prywatnych (brak dostępu).

- **Uprawnienia:**
  - `like` oraz `save` dostępne tylko dla zalogowanych; niezalogowani po kliknięciu widzą modal logowania
  - `edit`/`delete` widoczne wyłącznie dla właściciela pina

### 6.3. Filtrowanie, wyszukiwanie i sortowanie

- **Pasek filtrów (u góry feedu):**
  - filtr instrumentu: Gitara / Bas / Perkusja
  - filtr gatunków jako chips (multi-select)
  - wyszukiwarka (tytuł/autor)
  - sortowanie: popularne (na podstawie algorytmu/like) / najnowsze
  - filtry można łączyć; wyniki są paginowane

### 6.4. Autoryzacja i konto

- **Logowanie (`LoginView`):** Formularz logowania e-mailem i hasłem. Po pomyślnym zalogowaniu, aplikacja zapisuje token `JWT` i nazwę użytkownika w `sessionStorage`, by w prawym górnym rogu strony wyświetlać inicjały użytkownika.
- **Rejestracja (`RegisterView`):** Samodzielny widok zbierający wymagane dane użytkownika, posiadający walidację długości i struktury hasła, z możliwością przejścia do logowania po sukcesie.

- **Profil użytkownika:**
  - nagłówek profilu: awatar, `display_name`, krótki opis/bio (opcjonalne), statystyki (liczba dodanych pinów, suma polubień otrzymanych, liczba kolekcji)
  - przycisk `edit profile` tylko dla właściciela profilu

- **Zakładki (tabs) na profilu:**
  - `Dodane piny` — grid jak w feedzie, z akcjami właściciela: `edit`, `delete` (opcjonalnie: `promote`)
  - `Polubione` — grid pinów polubionych przez użytkownika; możliwość szybkiego `unlike` oraz `save` do kolekcji

### 6.5. Upload i edycja pina

- **Widok `Upload`:** formularz przesłania pliku `.gp` oraz metadanych (tytuł, gatunki). Instrument jest wykrywany automatycznie z pliku.

- **Widok `Edit Pin`:** edycja metadanych (np. tytuł, gatunki, widoczność); dostępny tylko dla właściciela.

### 6.6. Kolekcje i social

- **Kolekcje:** użytkownik może tworzyć i zarządzać kolekcjami oraz dodawać do nich piny (`save`). UI powinno umożliwiać wybór kolekcji z listy (dropdown/modal) oraz podstawowe operacje (utwórz/zmień nazwę/usuń kolekcję) zgodnie z `CollectionService`.

- **Udostępnianie (`share`):** kopiowanie linku do pina, opcjonalnie systemowe udostępnianie (Web Share API) na mobile.

- **Polubienia:** ikona „serduszko” przełącza stan polubienia; licznik polubień aktualizuje się po akcji.

---

## 7. Architektura i Decyzje Projektowe (ADR)
Wszelkie kluczowe decyzje technologiczne i architektoniczne podejmowane w projekcie są dokumentowane w formacie **ADR (Architecture Decision Records)**. Znajdują się one w katalogu `specs/adr/`.

Obecnie zarejestrowane decyzje:
* **[ADR-0001](../adr/0001-record-architecture-decisions.md):** Zapisywanie decyzji architektonicznych (ADR)
* **[ADR-0002](../adr/0002-use-vue3-for-frontend.md):** Wybór Vue 3 na framework frontendowy
* **[ADR-0003](../adr/0003-use-dotnet-8-web-api-for-backend.md):** Wybór C# i ASP.NET Core (.NET 8) do budowy API
* **[ADR-0004](../adr/0004-use-ef-core-and-sql-server.md):** Użycie MS SQL Server oraz Entity Framework Core
* **[ADR-0005](../adr/0005-use-jwt-for-stateless-auth.md):** Użycie JWT (JSON Web Tokens) do bezstanowej autoryzacji
* **[ADR-0006](../adr/0006-use-alphatab-for-score-rendering.md):** Użycie biblioteki AlphaTab do renderowania tabulatur