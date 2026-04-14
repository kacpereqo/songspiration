using System;
using System.Linq;
using System.Threading.Tasks;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.Models;

namespace SongSpiration.BLL.Services;

public class UserService : IUserService
{
    // In a real application, these would be injected repositories or a DB context
    // For Version I, we might use mock implementation or placeholders
    
    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
    {
        // Placeholder for registration logic
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = "hashed_" + registerDto.Password, // Simple placeholder
            CreatedAt = DateTime.UtcNow
        };

        return new AuthResponseDto
        {
            AccessToken = "mock_access_token",
            RefreshToken = "mock_refresh_token",
            User = MapToDto(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Placeholder for login logic
        return new AuthResponseDto
        {
            AccessToken = "mock_access_token",
            RefreshToken = "mock_refresh_token",
            User = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = loginDto.Email,
                DisplayName = "Logged In User"
            }
        };
    }

    public async Task<UserDto?> GetProfileAsync(Guid userId)
    {
        return new UserDto
        {
            Id = userId,
            Email = "user@example.com",
            DisplayName = "Example User"
        };
    }

    public async Task<UserDto> UpdateProfileAsync(Guid userId, UserDto updateDto)
    {
        updateDto.Id = userId;
        return updateDto;
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Roles = user.Roles,
            CreatedAt = user.CreatedAt
        };
    }
}
