namespace SongSpiration.BLL.Services
{
    public class CollectionService
    {
        public CollectionService()
        {
        }

        public async Task CreateCollection(string title, string description, Guid userId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }

        public async Task GetCollectionById(Guid collectionId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }

        public async Task AddPinToCollection(Guid collectionId, Guid pinId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }

        public async Task RemovePinFromCollection(Guid collectionId, Guid pinId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }
    }
}