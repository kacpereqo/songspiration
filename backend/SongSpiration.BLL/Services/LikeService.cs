namespace SongSpiration.BLL.Services
{
    public class LikeService
    {
        public LikeService()
        {
        }

        public async Task Like(Guid pinId, Guid userId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }

        public async Task Unlike(Guid pinId, Guid userId)
        {
            // Implementacja tymczasowa - brak repozytorium
            await Task.CompletedTask;
        }

        public async Task<bool> IsLiked(Guid pinId, Guid userId)
        {
            // Implementacja tymczasowa - brak repozytorium
            return await Task.FromResult(false);
        }
    }
}