using SongSpiration.BLL.DTOs;
using SongSpiration.Models;

namespace SongSpiration.BLL.Interfaces
{
    public interface IAdminPanelService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(UserCreateDto userCreateDto);
        Task UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(int userId);
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(int genreId);
        Task<Genre> CreateGenreAsync(GenreCreateDto genreCreateDto);
        Task UpdateGenreAsync(int genreId, GenreCreateDto genreCreateDto);
        Task DeleteGenreAsync(int genreId);
        Task<List<User>> SearchUsersAsync(string searchTerm);
        Task BanUserAsync(int userId);
        Task DeletePinsForUserAsync(int userId);
    }
}