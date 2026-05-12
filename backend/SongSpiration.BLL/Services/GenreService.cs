using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.BLL.DTOs;

namespace SongSpiration.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllGenresAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _genreRepository.GetGenreByIdAsync(id);
        }

        public async Task<Genre> CreateGenreAsync(GenreCreateDto genreCreateDto)
        {
            var genre = new Genre
            {
                Name = genreCreateDto.Name
            };
            return await _genreRepository.CreateGenreAsync(genre);
        }

        public async Task<Genre> UpdateGenreAsync(int id, GenreCreateDto genreCreateDto)
        {
            var genre = await _genreRepository.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return null;
            }

            genre.Name = genreCreateDto.Name;
            return await _genreRepository.UpdateGenreAsync(genre);
        }

        public async Task DeleteGenreAsync(int id)
        {
            await _genreRepository.DeleteGenreAsync(id);
        }
    }
}