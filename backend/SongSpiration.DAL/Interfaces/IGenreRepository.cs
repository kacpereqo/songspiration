using SongSpiration.Models;

namespace SongSpiration.DAL.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(int id);
        Task<Genre> CreateGenreAsync(Genre genre);
        Task<Genre> UpdateGenreAsync(Genre genre);
        Task DeleteGenreAsync(int id);
    }
}