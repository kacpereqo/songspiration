using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.Models;

namespace SongSpiration.BLL.Services;

public class PinService : IPinService
{
    public async Task<PinDto> CreatePinAsync(Guid ownerId, CreatePinDto createDto)
    {
        // TODO: Implement actual pin creation logic with database persistence
        var pin = new Pin
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Title = createDto.Title,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            Instrument = Instrument.Guitar,
            Filename = "default.gp",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await Task.FromResult(MapToDto(pin));
    }

    public async Task<PinDto?> GetPinByIdAsync(Guid pinId)
    {
        // TODO: Implement actual pin retrieval logic from database
        return await Task.FromResult<PinDto?>(null);
    }

    public async Task<PinDto> UpdatePinAsync(Guid pinId, UpdatePinDto updateDto)
    {
        // TODO: Implement actual pin update logic
        var updatedPin = new PinDto
        {
            Id = pinId,
            Title = updateDto.Title ?? "",
            Description = updateDto.Description,
            Visibility = updateDto.Visibility ?? PinVisibility.Public,
            Instrument = Instrument.Guitar,
            Filename = "updated.gp",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        return await Task.FromResult(updatedPin);
    }

    public async Task<bool> DeletePinAsync(Guid pinId)
    {
        // TODO: Implement actual pin deletion logic
        return await Task.FromResult(false);
    }

    public async Task<IEnumerable<PinDto>> GetAllPinsAsync()
    {
        // TODO: Implement actual retrieval of all pins from database
        return await Task.FromResult<IEnumerable<PinDto>>(new List<PinDto>());
    }

    public async Task<IEnumerable<PinDto>> GetPinsByUserIdAsync(Guid userId)
    {
        // TODO: Implement actual retrieval of pins by user ID from database
        return await Task.FromResult<IEnumerable<PinDto>>(new List<PinDto>());
    }

    public async Task<IEnumerable<PinDto>> GetPinsByBoardIdAsync(Guid boardId)
    {
        // TODO: Implement actual retrieval of pins by board ID from database
        return await Task.FromResult<IEnumerable<PinDto>>(new List<PinDto>());
    }

    private PinDto MapToDto(Pin pin)
    {
        return new PinDto
        {
            Id = pin.Id,
            OwnerId = pin.OwnerId,
            Title = pin.Title,
            Description = pin.Description,
            Instrument = pin.Instrument,
            Visibility = pin.Visibility,
            Filename = pin.Filename,
            Size = pin.Size,
            CreatedAt = pin.CreatedAt,
            Genres = pin.PinGenres?.Select(pg => pg.Genre?.Name ?? "").ToList() ?? new List<string>()
        };
    }
}
