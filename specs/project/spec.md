# Specyfikacja Projektu: SongSpiration (Wersja Rozszerzona)

## 1. Cel Projektu
**SongSpiration** to aplikacja webowa służąca do kolekcjonowania i przeglądania inspiracji muzycznych w formie wizualnych "pinów". Platforma dedykowana jest muzykom, umożliwiając im kategoryzację pomysłów, podgląd nut oraz współdzielenie plików tabulatur.

---

## 2. Stos Technologiczny
* **Frontend:** Vue 3
* **Backend:** C# / ASP.NET Core Web API (.NET 10/8)
* **Baza danych:** SQL Server + Entity Framework Core

---

## 3. Architektura Systemu i Funkcjonalności

### 3.1. System Użytkowników i Autoryzacja
* **Rejestracja i Logowanie:** Obsługa kont użytkowników (JWT Auth).
* **Profil Użytkownika:** Możliwość zarządzania własnymi przesłanymi plikami i ustawieniami profilu.

### 3.2. Zarządzanie Treścią (Pins & Tabs)
* **Upload:** Formularz przesyłania plików `.gp` wraz z metadanymi - tytuł, gatunki muzyczne.
* **Automatyzacja:** Instrument zostanie wybrany samemu na podstawie przesłanego pliku `.gp`.
* **Podgląd wizualny:** Przechowywanie grafik reprezentujących zapis nutowy dla każdego pinu.

### 3.3. System Filtrowania i Wyszukiwania
* **Filtrowanie po Instrumencie:** Szybki wybór kategorii: Gitara, Bas, Perkusja.
* **Filtrowanie po Gatunku:** Możliwość kategoryzowania pinów według gatunków muzycznych.
* **Sortowanie:** Sortowanie po popularności, polubieniach lub według daty publikacji.

---

## 4. Zadania Warstwy Logiki Biznesowej
* **Service Layer:** Implementacja serwisów `UserService`, `PinService` oraz `CollectionService`.
* **Unit Testing:** Testy rejestracji, mechanizmu filtrowania oraz zapisu plików.

---

## 5. Model Danych (Wysokopoziomowy)
| Tabela | Opis |
|---|---|
| `User` | Dane konta, profil (display_name, avatar), role. |
| `Pin` | Metadane pina, właściciel, lokalizacja pliku `.gp`. |
| `Genre` | Lista gatunków muzycznych. |
| `Like` | Relacja polubień użytkownik ↔ pin. |

---

## 6. Interfejs Użytkownika (UI)

### 6.1. Widoki publiczne i nawigacja
- **Navbar:** Logo, wyszukiwarka, skróty do `Upload` i `Profilu`.
- **Strona główna:** Responsywny grid pinów z infinite scroll.

### 6.2. Karta pina i szczegóły
- **Karta:** Miniatura, tytuł, autor, akcje (`play`, `like`, `save`).
- **Szczegóły:** Pełny podgląd AlphaTab, odtwarzacz MIDI, edycja (dla właściciela).

### 6.3. Filtrowanie i wyszukiwanie
- Pasek filtrów: instrumenty, gatunki (chips), wyszukiwarka tekstowa.

### 6.4. Widok Profilu i Zarządzanie Kontem

Strona profilu (`/profile/{id}` lub `/profile/me`) służy do prezentacji tożsamości użytkownika, jego statystyk oraz zgromadzonych treści muzycznych.

#### 6.4.1. Struktura Wizualna i Nawigacja
* **Dostęp:** Wejście na profil następuje poprzez interakcję z elementami nawigacyjnymi (avatar/nazwa) w górnym pasku aplikacji.
* **Sekcja nagłówka (Header):** * Prezentacja danych publicznych: zdjęcie profilowe (Avatar), nazwa wyświetlana (`DisplayName`) oraz opis biograficzny (`Bio`).
    * Statystyki aktywności: łączna liczba dodanych pinów oraz suma otrzymanych polubień.
    * Przycisk akcji **"Edytuj profil"**, dostępny dla właściciela konta.
* **Przełącznik widoku (Tabs):** Interfejs umożliwiający zmianę wyświetlanej zawartości w obrębie tej samej strony:
    * **Dodane:** Grid z pinami utworzonymi przez użytkownika.
    * **Polubione:** Grid z pinami, które użytkownik oznaczył jako ulubione.

#### 6.4.2. Funkcjonalność i Integracja z BLL
* **Prezentacja Treści:** Widok wykorzystuje `GetPinsByUserIdAsync` do pobierania własnych publikacji oraz odpowiednie filtry dla treści polubionych.
* **Zarządzanie Profilem:** * Formularz edycji umożliwia aktualizację pól `DisplayName`, `AvatarUrl` oraz `Bio`.
    * Proces aktualizacji danych jest obsługiwany przez metodę `UpdateProfileAsync`.
* **Bezpieczeństwo i Usuwanie:** * Wyświetlanie przycisków edycji i usuwania jest uzależnione od uprawnień zalogowanego użytkownika (widoczne tylko dla właściciela).
    * Możliwość trwałego usunięcia konta poprzez funkcję `DeleteAccountAsync`.

### 6.5. Upload i edycja pina
- Formularz przesyłania i edycji metadanych pina (tylko właściciel).

### 6.6. Kolekcje i social
- Zarządzanie kolekcjami, udostępnianie linków, system polubień.