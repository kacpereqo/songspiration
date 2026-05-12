using SongSpiration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.BLL.Interfaces
{
    public interface IAdminPanelService
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(int id);
        Task<Genre> CreateGenreAsync(GenreCreateDto genreCreateDto);
        Task<Genre> UpdateGenreAsync(int id, GenreCreateDto genreCreateDto);
        Task DeleteGenreAsync(int id);
        Task<List<User>> SearchUsersAsync(string criteria);
        Task DeleteUserAsync(int id);
        Task BanUserAsync(int id);
        Task DeletePinsForUserAsync(int id);
    }
}