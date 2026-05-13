# Specyfikacja Panelu Administracyjnego

## Przegląd
Panel administracyjny jest interfejsem opartym na przeglądarce internetowej, przeznaczonym dla administratorów do zarządzania gatunkami muzycznymi i kontami użytkowników w aplikacji. Umożliwia on funkcjonalność CRUD (Create, Read, Update, Delete) dla gatunków muzycznych oraz różne funkcje zarządzania użytkownikami.

## Funkcje

### 1. Uwierzytelnianie Administratora
- **Opis**: Administratorzy muszą się zalogować, aby uzyskać dostęp do panelu administracyjnego.
- **Wymagania**:
  - Po pomyślnym zalogowaniu administrator jest przekierowywany do panelu administracyjnego.
  - Ścieżka URL dla panelu administracyjnego to `/admin`.
  - Na stronie panelu administracyjnego wyświetlany jest napis "Admin".

### 2. Zarządzanie Gatunkami Muzycznymi
- **Opis**: Administratorzy mogą wykonywać operacje CRUD na gatunkach muzycznych.
- **Wymagania**:
  - **Tworzenie**: Administratorzy mogą dodawać nowe gatunki muzyczne do bazy danych.
  - **Odczyt**: Administratorzy mogą przeglądać listę wszystkich gatunków muzycznych.
  - **Aktualizacja**: Administratorzy mogą edytować istniejące gatunki muzyczne.
  - **Usuwanie**: Administratorzy mogą usuwać gatunki muzyczne z bazy danych.

### 3. Zarządzanie Użytkownikami
- **Opis**: Administratorzy mogą wyszukiwać użytkowników i wykonywać różne działania na kontach użytkowników.
- **Wymagania**:
  - **Wyszukiwanie Użytkowników**: Administratorzy mogą wyszukiwać użytkowników według nazwy użytkownika, adresu e-mail lub innych odpowiednich kryteriów.
  - **Usuwanie Kont**: Administratorzy mogą usuwać konta użytkowników.
  - **Banowanie Kont**: Administratorzy mogą banować konta użytkowników, uniemożliwiając im dostęp do aplikacji.
  - **Usuwanie Pinów**: Administratorzy mogą usuwać piny (konkretne punkty danych) powiązane z kontami użytkowników.

## Interfejs Użytkownika

### 1. Strona Logowania
- **Opis**: Strona logowania, na której administratorzy wprowadzają swoje dane uwierzytelniające, aby uzyskać dostęp do panelu administracyjnego.
- **Elementy**:
  - Pole wprowadzania nazwy użytkownika
  - Pole wprowadzania hasła
  - Przycisk logowania
  - Wyświetlanie komunikatu o błędzie dla nieprawidłowych danych uwierzytelniających

### 2. Pulpit Panelu Administracyjnego
- **Opis**: Główny pulpit panelu administracyjnego, dostępny pod adresem `/admin`.
- **Elementy**:
  - Menu nawigacyjne z linkami do różnych sekcji (Gatunki Muzyczne, Zarządzanie Użytkownikami)
  - Napis "Admin" wyświetlany wyraźnie
  - Krótkie statystyki lub podsumowanie ostatnich działań

### 3. Sekcja Zarządzania Gatunkami Muzycznymi
- **Opis**: Sekcja poświęcona zarządzaniu gatunkami muzycznymi.
- **Elementy**:
  - Lista gatunków muzycznych z opcjami edycji lub usunięcia każdego gatunku
  - Formularz do dodania nowego gatunku muzycznego
  - Pasek wyszukiwania do filtrowania gatunków muzycznych

### 4. Sekcja Zarządzania Użytkownikami
- **Opis**: Sekcja poświęcona zarządzaniu kontami użytkowników.
- **Elementy**:
  - Pasek wyszukiwania do znajdowania użytkowników
  - Lista wyników wyszukiwania z opcjami usunięcia, zbanowania lub wyświetlenia pinów dla każdego użytkownika
  - Okna dialogowe potwierdzające usunięcie lub zbanowanie kont

## Szczegóły Techniczne

### 1. Backend
- **Opis**: Backend obsługuje uwierzytelnianie, operacje CRUD dla gatunków muzycznych oraz działania zarządzania użytkownikami.
- **Technologie**:
  - Język Programowania: C#
  - Framework: ASP.NET Core
  - Baza Danych: SQL Server
  - Uwierzytelnianie: JWT (JSON Web Tokens)

### 2. Frontend
- **Opis**: Frontend zapewnia interfejs użytkownika dla panelu administracyjnego.
- **Technologie**:
  - Framework: Vue.js
  - Stylizacja: CSS (z możliwością użycia preprocesorów takich jak SASS)
  - Zarządzanie Stanem: Vuex
  - Klient HTTP: Axios

### 3. Punkty Końcowe API
- **Opis**: Backend udostępnia punkty końcowe API typu RESTful, z którymi frontend może się komunikować.
- **Punkty Końcowe**:
  - `POST /api/auth/login`: Logowanie administratora
  - `GET /api/genres`: Pobierz listę gatunków muzycznych
  - `POST /api/genres`: Dodaj nowy gatunek muzyczny
  - `PUT /api/genres/{id}`: Zaktualizuj istniejący gatunek muzyczny
  - `DELETE /api/genres/{id}`: Usuń gatunek muzyczny
  - `GET /api/users/search`: Wyszukaj użytkowników
  - `DELETE /api/users/{id}`: Usuń konto użytkownika
  - `POST /api/users/{id}/ban`: Zbanuj konto użytkownika
  - `DELETE /api/users/{id}/pins`: Usuń piny użytkownika

## Uwagi Dotyczące Bezpieczeństwa
- **Opis**: Zapewnienie, że panel administracyjny jest bezpieczny i chroni poufne dane.
- **Środki**:
  - Używaj HTTPS do wszystkich komunikacji.
  - Wdroż kontrolę dostępu opartą na rolach (RBAC), aby ograniczyć dostęp do panelu administracyjnego.
  - Zweryfikuj i oczyść wszystkie dane wejściowe użytkownika, aby zapobiec atakom SQL injection i XSS.
  - Używaj bezpiecznych algorytmów hashowania haseł do przechowywania danych uwierzytelniających administratorów.

## Podsumowanie
Specyfikacja panelu administracyjnego obejmuje funkcje, interfejs użytkownika, szczegóły techniczne i uwagi dotyczące bezpieczeństwa dla zarządzania gatunkami muzycznymi i kontami użytkowników. Ta specyfikacja służy jako kompleksowy przewodnik po rozwoju panelu administracyjnego.