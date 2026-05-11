using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models.Entities;

namespace SongSpiration.DAL.Repositories;

public class PinRepository : IPinRepository
{
    private readonly SongSpirationDbContext _db;

    public PinRepository(SongSpirationDbContext db) => _db = db;

    public async Task AddAsync(Pin pin) => await _db.Pins.AddAsync(pin);

    public void Remove(Pin pin)
    {
        var trackedEntity = _db.Pins.Local.FirstOrDefault(p => p.Id == pin.Id);
        if (trackedEntity != null)
        {
            _db.Entry(trackedEntity).State = EntityState.Detached;
        }
        _db.Pins.Remove(pin);
    }

    public void Update(Pin pin) => _db.Pins.Update(pin);

    public async Task<Pin?> GetByIdAsync(Guid id)
    {
        return await _db.Pins
            .AsNoTracking() 
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pin?> GetByIdWithDetailsAsync(Guid id)
        => await _db.Pins
            .Include(p => p.Owner)
            .Include(p => p.PinGenres)
                .ThenInclude(pg => pg.Genre)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Pin>> GetPinsAsync(int skip = 0, int take = 50)
    {
        return await _db.Pins
            .AsNoTracking()
            .Include(p => p.Owner)
            .Include(p => p.PinGenres)
                .ThenInclude(pg => pg.Genre)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<bool> ToggleLikeAsync(Guid userId, Guid pinId)
    {
        var existing = await _db.Likes.FindAsync(userId, pinId);
        if (existing is null)
        {
            var like = new Like { UserId = userId, PinId = pinId, CreatedAt = DateTime.UtcNow };
            await _db.Likes.AddAsync(like);
            await _db.SaveChangesAsync();
            return true;
        }
        else
        {
            _db.Likes.Remove(existing);
            await _db.SaveChangesAsync();
            return false;
        }
    }

    public async Task<int> GetCountByUserIdAsync(Guid userId) 
        => await _db.Pins.CountAsync(p => p.OwnerId == userId);

    public async Task<int> GetTotalLikesReceivedByUserIdAsync(Guid userId)
        => await _db.Likes.CountAsync(l => l.Pin.OwnerId == userId);

    public async Task<IEnumerable<Pin>> GetPinsByUserIdAsync(Guid userId) 
    {
        return await _db.Pins
            .Where(p => p.OwnerId == userId) 
            .Include(p => p.PinGenres)
                .ThenInclude(pg => pg.Genre)
            .ToListAsync();
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}