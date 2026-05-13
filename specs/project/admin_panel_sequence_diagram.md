sequenceDiagram
    participant Admin
    participant AdminController
    participant GenreService
    participant GenreRepository
    participant UserService
    participant UserRepository

    Admin->>AdminController: Logowanie
    AdminController->>AdminController: Weryfikuj dane uwierzytelniające
    AdminController->>Admin: Zwróć token JWT

    Admin->>AdminController: Pobierz listę gatunków muzycznych
    AdminController->>GenreService: Pobierz listę gatunków muzycznych
    GenreService->>GenreRepository: Pobierz listę gatunków muzycznych
    GenreRepository->>GenreService: Zwróć listę gatunków muzycznych
    GenreService->>AdminController: Zwróć listę gatunków muzycznych
    AdminController->>Admin: Zwróć listę gatunków muzycznych

    Admin->>AdminController: Dodaj nowy gatunek muzyczny
    AdminController->>GenreService: Dodaj nowy gatunek muzyczny
    GenreService->>GenreRepository: Dodaj nowy gatunek muzyczny
    GenreRepository->>GenreService: Potwierdź dodanie gatunku muzycznego
    GenreService->>AdminController: Potwierdź dodanie gatunku muzycznego
    AdminController->>Admin: Potwierdź dodanie gatunku muzycznego

    Admin->>AdminController: Zaktualizuj gatunek muzyczny
    AdminController->>GenreService: Zaktualizuj gatunek muzyczny
    GenreService->>GenreRepository: Zaktualizuj gatunek muzyczny
    GenreRepository->>GenreService: Potwierdź aktualizację gatunku muzycznego
    GenreService->>AdminController: Potwierdź aktualizację gatunku muzycznego
    AdminController->>Admin: Potwierdź aktualizację gatunku muzycznego

    Admin->>AdminController: Usuń gatunek muzyczny
    AdminController->>GenreService: Usuń gatunek muzyczny
    GenreService->>GenreRepository: Usuń gatunek muzyczny
    GenreRepository->>GenreService: Potwierdź usunięcie gatunku muzycznego
    GenreService->>AdminController: Potwierdź usunięcie gatunku muzycznego
    AdminController->>Admin: Potwierdź usunięcie gatunku muzycznego

    Admin->>AdminController: Wyszukaj użytkowników
    AdminController->>UserService: Wyszukaj użytkowników
    UserService->>UserRepository: Wyszukaj użytkowników
    UserRepository->>UserService: Zwróć listę użytkowników
    UserService->>AdminController: Zwróć listę użytkowników
    AdminController->>Admin: Zwróć listę użytkowników

    Admin->>AdminController: Usuń konto użytkownika
    AdminController->>UserService: Usuń konto użytkownika
    UserService->>UserRepository: Usuń konto użytkownika
    UserRepository->>UserService: Potwierdź usunięcie konta użytkownika
    UserService->>AdminController: Potwierdź usunięcie konta użytkownika
    AdminController->>Admin: Potwierdź usunięcie konta użytkownika

    Admin->>AdminController: Zbanuj konto użytkownika
    AdminController->>UserService: Zbanuj konto użytkownika
    UserService->>UserRepository: Zbanuj konto użytkownika
    UserRepository->>UserService: Potwierdź zbanowanie konta użytkownika
    UserService->>AdminController: Potwierdź zbanowanie konta użytkownika
    AdminController->>Admin: Potwierdź zbanowanie konta użytkownika

    Admin->>AdminController: Usuń piny użytkownika
    AdminController->>UserService: Usuń piny użytkownika
    UserService->>UserRepository: Usuń piny użytkownika
    UserRepository->>UserService: Potwierdź usunięcie pinów użytkownika
    UserService->>AdminController: Potwierdź usunięcie pinów użytkownika
    AdminController->>Admin: Potwierdź usunięcie pinów użytkownika