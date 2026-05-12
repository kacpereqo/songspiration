classDiagram
    class AdminController {
        +Logowanie(username: string, password: string): string
        +PobierzListęGatunkówMuzycznych(): List~Genre~
        +DodajNowyGatunekMuzyczny(genre: Genre): void
        +ZaktualizujGatunekMuzyczny(id: int, genre: Genre): void
        +UsuńGatunekMuzyczny(id: int): void
        +WyszukajUżytkowników(criteria: string): List~User~
        +UsuńKontoUżytkownika(id: int): void
        +ZbanujKontoUżytkownika(id: int): void
        +UsuńPinyUżytkownika(id: int): void
    }

    class GenreService {
        +PobierzListęGatunkówMuzycznych(): List~Genre~
        +DodajNowyGatunekMuzyczny(genre: Genre): void
        +ZaktualizujGatunekMuzyczny(id: int, genre: Genre): void
        +UsuńGatunekMuzyczny(id: int): void
    }

    class GenreRepository {
        +PobierzListęGatunkówMuzycznych(): List~Genre~
        +DodajNowyGatunekMuzyczny(genre: Genre): void
        +ZaktualizujGatunekMuzyczny(id: int, genre: Genre): void
        +UsuńGatunekMuzyczny(id: int): void
    }

    class UserService {
        +WyszukajUżytkowników(criteria: string): List~User~
        +UsuńKontoUżytkownika(id: int): void
        +ZbanujKontoUżytkownika(id: int): void
        +UsuńPinyUżytkownika(id: int): void
    }

    class UserRepository {
        +WyszukajUżytkowników(criteria: string): List~User~
        +UsuńKontoUżytkownika(id: int): void
        +ZbanujKontoUżytkownika(id: int): void
        +UsuńPinyUżytkownika(id: int): void
    }

    class Genre {
        +Id: int
        +Name: string
    }

    class User {
        +Id: int
        +Username: string
        +Email: string
    }

    AdminController --> GenreService
    AdminController --> UserService
    GenreService --> GenreRepository
    UserService --> UserRepository
    GenreRepository --> Genre
    UserRepository --> User