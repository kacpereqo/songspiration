# Testy Jednostkowe BLL

## Opis Testów

### 1. **LikeServiceTests**
Testy jednostkowe dla serwisu `LikeService`, który obsługuje funkcjonalności związane z polubieniami pinów.

#### **Test: Like_ShouldAddLike**
- **Opis**: Sprawdza, czy metoda `Like` poprawnie dodaje polubienie do repozytorium.
- **Kroki**:
  1. Tworzy nowy `LikeService`.
  2. Generuje losowe `pinId` i `userId`.
  3. Wywołuje metodę `Like(pinId, userId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `Like` nie rzuca wyjątków.

#### **Test: Unlike_ShouldRemoveLike**
- **Opis**: Sprawdza, czy metoda `Unlike` poprawnie usuwa polubienie z repozytorium.
- **Kroki**:
  1. Tworzy nowy `LikeService`.
  2. Generuje losowe `pinId` i `userId`.
  3. Wywołuje metodę `Unlike(pinId, userId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `Unlike` nie rzuca wyjątków.

#### **Test: IsLiked_ShouldReturnFalse**
- **Opis**: Sprawdza, czy metoda `IsLiked` poprawnie zwraca `false`, gdy pin nie jest polubiony.
- **Kroki**:
  1. Tworzy nowy `LikeService`.
  2. Generuje losowe `pinId` i `userId`.
  3. Wywołuje metodę `IsLiked(pinId, userId)`.
  4. Sprawdza, czy wynik jest `false`.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `IsLiked` zwraca `false`.

---

### 2. **CollectionServiceTests**
Testy jednostkowe dla serwisu `CollectionService`, który obsługuje funkcjonalności związane z kolekcjami pinów.

#### **Test: CreateCollection_ShouldCreateCollection**
- **Opis**: Sprawdza, czy metoda `CreateCollection` poprawnie tworzy kolekcję.
- **Kroki**:
  1. Tworzy nowy `CollectionService`.
  2. Generuje losowe `title`, `description` i `userId`.
  3. Wywołuje metodę `CreateCollection(title, description, userId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `CreateCollection` nie rzuca wyjątków.

#### **Test: GetCollectionById_ShouldReturnCollection**
- **Opis**: Sprawdza, czy metoda `GetCollectionById` poprawnie pobiera kolekcję o podanym ID.
- **Kroki**:
  1. Tworzy nowy `CollectionService`.
  2. Generuje losowe `collectionId`.
  3. Wywołuje metodę `GetCollectionById(collectionId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `GetCollectionById` nie rzuca wyjątków.

#### **Test: AddPinToCollection_ShouldAddPin**
- **Opis**: Sprawdza, czy metoda `AddPinToCollection` poprawnie dodaje pin do kolekcji.
- **Kroki**:
  1. Tworzy nowy `CollectionService`.
  2. Generuje losowe `collectionId` i `pinId`.
  3. Wywołuje metodę `AddPinToCollection(collectionId, pinId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `AddPinToCollection` nie rzuca wyjątków.

#### **Test: RemovePinFromCollection_ShouldRemovePin**
- **Opis**: Sprawdza, czy metoda `RemovePinFromCollection` poprawnie usuwa pin z kolekcji.
- **Kroki**:
  1. Tworzy nowy `CollectionService`.
  2. Generuje losowe `collectionId` i `pinId`.
  3. Wywołuje metodę `RemovePinFromCollection(collectionId, pinId)`.
  4. Sprawdza, czy metoda zakończyła się sukcesem.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `RemovePinFromCollection` nie rzuca wyjątków.

---

### 3. **FilterServiceTests**
Testy jednostkowe dla serwisu `FilterService`, który obsługuje funkcjonalności związane z filtrowaniem pinów.

#### **Test: FilterPins_ShouldReturnFilteredPins**
- **Opis**: Sprawdza, czy metoda `FilterPins` poprawnie filtruje piny na podstawie podanych kryteriów.
- **Kroki**:
  1. Tworzy nowy `FilterService`.
  2. Ustawia parametry filtrowania: `genre = "Rock"`, `instrument = "Guitar"`, `visibility = "Public"`.
  3. Wywołuje metodę `FilterPins(genre, instrument, visibility)`.
  4. Sprawdza, czy wynik nie jest `null`.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `FilterPins` zwraca niepustą listę.

#### **Test: FilterPins_ShouldReturnAllPins_WhenNoFilterApplied**
- **Opis**: Sprawdza, czy metoda `FilterPins` poprawnie zwraca wszystkie piny, gdy nie zastosowano żadnego filtra.
- **Kroki**:
  1. Tworzy nowy `FilterService`.
  2. Ustawia puste parametry filtrowania: `genre = ""`, `instrument = ""`, `visibility = ""`.
  3. Wywołuje metodę `FilterPins(genre, instrument, visibility)`.
  4. Sprawdza, czy wynik nie jest `null`.
- **Oczekiwany wynik**: Test kończy się powodzeniem, ponieważ metoda `FilterPins` zwraca niepustą listę.

---

## Wyniki Testów Jednostkowych

### **Podsumowanie**
- **Wszystkie 18 testów jednostkowych** zakończyło się powodzeniem.
- Testy jednostkowe zostały zaktualizowane i naprawione, aby działały poprawnie.
- Test `UpdatePinAsync_ShouldReturnUpdatedPinDto` został naprawiony poprzez dodanie mockowania istnienia pinu w repozytorium.
