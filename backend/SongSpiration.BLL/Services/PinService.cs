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
        // TODO: Implement actual pin creation logic
        var pin = new Pin
        {
            Id = Guid.NewGuid(), // This should be set by the database or repository
            OwnerId = ownerId,
            Title = createDto.Title,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            // Instrument and Filename would typically be determined during file upload/analysis
            Instrument = Instrument.Guitar, // Placeholder
            Filename = "default.gp", // Placeholder
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // After saving to DB, map to DTO
        return MapToDto(pin);
    }

    public async Task<PinDto?> GetPinByIdAsync(Guid pinId)
    {
        // TODO: Implement actual pin retrieval logic
        // Placeholder for now
        return new PinDto
        {
            Id = pinId,
            Title = "Sample Pin",
            OwnerName = "Test User", // This should come from a user service
            Instrument = Instrument.Guitar,
            Genres = new List<string> { "Rock", "Blues" }
        };
    }

    public async Task<PinDto> UpdatePinAsync(Guid pinId, UpdatePinDto updateDto)
    {
        // TODO: Implement actual pin update logic
        // Placeholder for now
        var updatedPin = new PinDto
        {
            Id = pinId,
            Title = updateDto.Title,
            Description = updateDto.Description,
            Visibility = updateDto.Visibility,
            Instrument = Instrument.Guitar, // Placeholder, should be existing value or updated if allowed
            Filename = "updated.gp", // Placeholder
            CreatedAt = DateTime.UtcNow.AddDays(-1), // Assuming it was created before
            UpdatedAt = DateTime.UtcNow
        };

        return updatedPin;
    }

    public async Task<bool> DeletePinAsync(Guid pinId)
    {
        // TODO: Implement actual pin deletion logic
        return await Task.FromResult(true); // Placeholder
    }

    public async Task<IEnumerable<PinDto>> GetAllPinsAsync()
    {
        // TODO: Implement actual retrieval of all pins
        // Placeholder for now
        return new List<PinDto>
        {
            new PinDto { Id = Guid.NewGuid(), Title = "All Pins 1", OwnerName = "User A" },
            new PinDto { Id = Guid.NewGuid(), Title = "All Pins 2", OwnerName = "User B" }
        };
    }

    public async Task<IEnumerable<PinDto>> GetPinsByUserIdAsync(Guid userId)
    {
        // TODO: Implement actual retrieval of pins by user ID
        // Placeholder for now
        return new List<PinDto>
        {
            new PinDto { Id = Guid.NewGuid(), Title = "User's Pin 1", OwnerName = "Current User", OwnerId = userId },
            new PinDto { Id = Guid.NewGuid(), Title = "User's Pin 2", OwnerName = "Current User", OwnerId = userId }
        };
    }

    public async Task<IEnumerable<PinDto>> GetPinsByBoardIdAsync(Guid boardId)
    {
        // TODO: Implement actual retrieval of pins by board ID
        // Placeholder for now
        return new List<PinDto>
        {
            new PinDto { Id = Guid.NewGuid(), Title = "Board Pin 1", OwnerName = "Board Owner" },
            new PinDto { Id = Guid.NewGuid(), Title = "Board Pin 2", OwnerName = "Board Owner" }
        };
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
