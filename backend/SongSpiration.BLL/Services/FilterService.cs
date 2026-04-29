namespace SongSpiration.BLL.Services
{
    public class FilterService
    {
        public FilterService()
        {
        }

        public async Task<IEnumerable<object>> FilterPins(string genre, string instrument, string visibility)
        {
            // Implementacja tymczasowa - brak repozytorium
            return await Task.FromResult(new List<object>());
        }
    }
}