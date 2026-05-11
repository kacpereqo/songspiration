using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.Models.Entities;

namespace SongSpiration.BLL.Services;

public class PinService : IPinService
{
    private readonly IPinRepository _pinRepository;

    public PinService(IPinRepository pinRepository)
    {
        _pinRepository = pinRepository;
    }

    public async Task<PinDto> CreatePinAsync(Guid ownerId, CreatePinDto createDto)
    {
        var pin = new Pin
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Title = createDto.Title,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            Instrument = createDto.Instrument,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PinGenres = new List<PinGenre>(),
            // Zapisujemy ścieżkę relatywną bezpośrednio z DTO
            FilePath = createDto.TempFileLocation,
            Filename = !string.IsNullOrEmpty(createDto.TempFileLocation) 
                ? Path.GetFileName(createDto.TempFileLocation) 
                : null,
            MimeType = "application/octet-stream"
        };

        try 
        {
            await _pinRepository.AddAsync(pin);
            await _pinRepository.SaveChangesAsync();

            if (createDto.GenreIds != null && createDto.GenreIds.Any())
            {
                foreach (var gId in createDto.GenreIds)
                {
                    pin.PinGenres.Add(new PinGenre 
                    { 
                        PinId = pin.Id, 
                        GenreId = gId 
                    });
                }
                await _pinRepository.SaveChangesAsync();
            }

            var pinWithDetails = await _pinRepository.GetByIdWithDetailsAsync(pin.Id);
            return MapToDto(pinWithDetails ?? pin);
        }
        catch (Exception)
        {
            // W razie błędu bazy danych, kontroler zajmie się ewentualnym czyszczeniem plików, 
            // jeśli przekażemy błąd dalej.
            throw;
        }
    }

    public async Task<PinDto?> GetPinByIdAsync(Guid pinId)
    {
        var pin = await _pinRepository.GetByIdWithDetailsAsync(pinId);
        return pin != null ? MapToDto(pin) : null;
    }

    public async Task<IEnumerable<PinDto>> GetPinsByUserIdAsync(Guid userId)
    {
        var pins = await _pinRepository.GetPinsByUserIdAsync(userId); 
        return pins.Select(pin => MapToDto(pin)).ToList();
    }   

    public async Task<IEnumerable<PinDto>> GetAllPinsAsync()
    {
        var pins = await _pinRepository.GetPinsAsync();
        return pins.Select(MapToDto);
    }

    public async Task<PinDto> UpdatePinAsync(Guid pinId, UpdatePinDto updateDto)
    {
        var existingPin = await _pinRepository.GetByIdWithDetailsAsync(pinId);
        if (existingPin == null) throw new KeyNotFoundException($"Pin o ID {pinId} nie istnieje.");

        if (updateDto.Title != null) existingPin.Title = updateDto.Title;
        if (updateDto.Description != null) existingPin.Description = updateDto.Description;
        if (updateDto.Visibility != null) existingPin.Visibility = updateDto.Visibility.Value;

        existingPin.UpdatedAt = DateTime.UtcNow;
        _pinRepository.Update(existingPin);
        await _pinRepository.SaveChangesAsync();

        return MapToDto(existingPin);
    }

    public async Task<bool> DeletePinAsync(Guid pinId)
    {
        var existingPin = await _pinRepository.GetByIdAsync(pinId);
        if (existingPin == null) return false;

        _pinRepository.Remove(existingPin);
        await _pinRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PinDto>> GetPinsByBoardIdAsync(Guid boardId)
    {
        return await Task.FromResult(Enumerable.Empty<PinDto>());
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
            FilePath = pin.FilePath,
            Size = pin.Size,
            CreatedAt = pin.CreatedAt,
            Genres = pin.PinGenres?
                .Where(pg => pg.Genre != null)
                .Select(pg => pg.Genre!.Name)
                .ToList() ?? new List<string>()
        };
    }
}