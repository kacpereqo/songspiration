using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SongSpiration.BLL.DTOs;

namespace SongSpiration.BLL.Interfaces;

public interface IPinService
{
    Task<PinDto> CreatePinAsync(Guid ownerId, CreatePinDto createDto);
    Task<PinDto?> GetPinAsync(Guid pinId);
    Task<IEnumerable<PinDto>> GetPinsAsync(PinFilterDto filter);
    Task<bool> DeletePinAsync(Guid userId, Guid pinId);
    Task<bool> ToggleLikeAsync(Guid userId, Guid pinId);
}
