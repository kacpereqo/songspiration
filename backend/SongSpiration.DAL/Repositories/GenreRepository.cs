using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using Microsoft.EntityFrameworkCore;

namespace SongSpiration.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DbContext _context;

        public GenreRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Set<Genre>().ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _context.Set<Genre>().FindAsync(id);
        }

        public async Task<Genre> CreateGenreAsync(Genre genre)
        {
            _context.Set<Genre>().Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> UpdateGenreAsync(Genre genre)
        {
            _context.Set<Genre>().Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Set<Genre>().FindAsync(id);
            if (genre != null)
            {
                _context.Set<Genre>().Remove(genre);
                await _context.SaveChangesAsync();
            }
        }
    }
}