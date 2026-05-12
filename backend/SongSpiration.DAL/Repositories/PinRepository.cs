using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using Microsoft.EntityFrameworkCore;

namespace SongSpiration.DAL.Repositories
{
    public class PinRepository : IPinRepository
    {
        private readonly DbContext _context;

        public PinRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<Pin>> GetAllPinsAsync()
        {
            return await _context.Set<Pin>().ToListAsync();
        }

        public async Task<Pin> GetPinByIdAsync(int id)
        {
            return await _context.Set<Pin>().FindAsync(id);
        }

        public async Task<Pin> CreatePinAsync(Pin pin)
        {
            _context.Set<Pin>().Add(pin);
            await _context.SaveChangesAsync();
            return pin;
        }

        public async Task<Pin> UpdatePinAsync(Pin pin)
        {
            _context.Set<Pin>().Update(pin);
            await _context.SaveChangesAsync();
            return pin;
        }

        public async Task DeletePinAsync(int id)
        {
            var pin = await _context.Set<Pin>().FindAsync(id);
            if (pin != null)
            {
                _context.Set<Pin>().Remove(pin);
                await _context.SaveChangesAsync();
            }
        }
    }
}