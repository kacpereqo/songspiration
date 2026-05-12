using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> SearchUsersAsync(string criteria)
        {
            return await _context.Set<User>().Where(u => EF.Functions.Like(u.Username, $"%{criteria}%") || EF.Functions.Like(u.Email, $"%{criteria}%")).ToListAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null)
            {
                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BanUserAsync(int id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null)
            {
                user.IsBanned = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePinsForUserAsync(int id)
        {
            var pins = await _context.Set<Pin>().Where(p => p.UserId == id).ToListAsync();
            _context.Set<Pin>().RemoveRange(pins);
            await _context.SaveChangesAsync();
        }
    }
}