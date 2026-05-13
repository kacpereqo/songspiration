using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DbContext _context;

        public GenreRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Set<Genre>().ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _context.Set<Genre>().FindAsync(id);
        }

        public async Task AddAsync(Genre genre)
        {
            await _context.Set<Genre>().AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public void Update(Genre genre)
        {
            _context.Set<Genre>().Update(genre);
        }

        public void Delete(Genre genre)
        {
            _context.Set<Genre>().Remove(genre);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
