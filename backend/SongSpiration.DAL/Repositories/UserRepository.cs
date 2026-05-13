using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;

namespace SongSpiration.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SongSpirationDbContext _db;

    public UserRepository(SongSpirationDbContext db) => _db = db;

    public async Task AddAsync(User user) => await _db.Users.AddAsync(user);

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.FindAsync(id);

    public void Update(User user) => _db.Users.Update(user);

    public void Delete(User user)
    {
        _db.Users.Remove(user);
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();

    public async Task<IEnumerable<User>> SearchUsersAsync(string criteria)
    {
        return await _db.Users
            .Where(u => u.Username.Contains(criteria) || u.Email.Contains(criteria))
            .ToListAsync();
    }

    public async Task BanUserAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsBanned = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteUserPinsAsync(int userId)
    {
        var pins = _db.Pins.Where(p => p.UserId == userId);
        _db.Pins.RemoveRange(pins);
        await _db.SaveChangesAsync();
    }
}
