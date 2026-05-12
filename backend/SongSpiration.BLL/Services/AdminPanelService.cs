using SongSpiration.BLL.Interfaces;
using SongSpiration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.BLL.Services
{
    public class AdminPanelService
    {
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;

        public AdminPanelService(IGenreService genreService, IUserService userService)
        {
            _genreService = genreService;
            _userService = userService;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _genreService.GetAllGenresAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _genreService.GetGenreByIdAsync(id);
        }

        public async Task<Genre> CreateGenreAsync(GenreCreateDto genreCreateDto)
        {
            return await _genreService.CreateGenreAsync(genreCreateDto);
        }

        public async Task<Genre> UpdateGenreAsync(int id, GenreCreateDto genreCreateDto)
        {
            return await _genreService.UpdateGenreAsync(id, genreCreateDto);
        }

        public async Task DeleteGenreAsync(int id)
        {
            await _genreService.DeleteGenreAsync(id);
        }

        public async Task<List<User>> SearchUsersAsync(string criteria)
        {
            return await _userService.SearchUsersAsync(criteria);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
        }

        public async Task BanUserAsync(int id)
        {
            await _userService.BanUserAsync(id);
        }

        public async Task DeletePinsForUserAsync(int id)
        {
            await _userService.DeletePinsForUserAsync(id);
        }
    }
}