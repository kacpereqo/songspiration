using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.BLL.DTOs;
using SongSpiration.Models.Entities; 

namespace SongSpiration.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IPinRepository _pinRepo;
    private readonly IEmailSender _emailSender;
    private readonly SongSpirationDbContext _dbContext;

    public UserService(IUserRepository userRepo, IPinRepository pinRepo, IEmailSender emailSender, SongSpirationDbContext dbContext)
    {
        _userRepo = userRepo;
        _pinRepo = pinRepo;
        _emailSender = emailSender;
        _dbContext = dbContext;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return null;

        return new UserProfileDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            AddedPinsCount = await _pinRepo.GetCountByUserIdAsync(userId),
            TotalLikesReceived = await _pinRepo.GetTotalLikesReceivedByUserIdAsync(userId)
        };
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDto updateDto)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return false;

        user.DisplayName = updateDto.DisplayName;
        user.AvatarUrl = updateDto.AvatarUrl;
        user.Bio = updateDto.Bio;

        _userRepo.Update(user);
        return await _userRepo.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAccountAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return false;

        _userRepo.Delete(user);
        return await _userRepo.SaveChangesAsync() > 0;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
    {
        var existingUser = await _userRepo.GetByEmailAsync(registerDto.Email);
        if (existingUser != null) throw new InvalidOperationException("Użytkownik o tym adresie e-mail już istnieje.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = "hashed_" + registerDto.Password,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return new AuthResponseDto { AccessToken = "token_" + user.Id, User = MapToDto(user) };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepo.GetByEmailAsync(loginDto.Email);
        if (user == null || user.PasswordHash != "hashed_" + loginDto.Password)
        {
            throw new UnauthorizedAccessException("Nieprawidłowy e-mail lub hasło.");
        }

        return new AuthResponseDto
        {
            AccessToken = "token_" + user.Id,
            User = MapToDto(user)
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null) return;

        var resetToken = Guid.NewGuid().ToString("N");

        var authToken = new AuthToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = resetToken,
            // Użycie pełnej ścieżki dla pewności kompilacji
            TokenType = TokenType.PasswordReset,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsRevoked = false
        };

        _dbContext.AuthTokens.Add(authToken);
        await _dbContext.SaveChangesAsync();

        var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";
        var message = $"<p><a href='{resetLink}'>Reset Password</a></p>";

        if (_emailSender != null) await _emailSender.SendEmailAsync(user.Email, "SongSpiration - Password Reset", message);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var tokenRecord = await _dbContext.AuthTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => 
                t.TokenHash == dto.Token && 
                t.TokenType == TokenType.PasswordReset &&
                !t.IsRevoked &&
                t.ExpiryDate > DateTime.UtcNow);

        if (tokenRecord == null || tokenRecord.User == null)
        {
            throw new InvalidOperationException("Invalid or expired reset token.");
        }

        tokenRecord.User.PasswordHash = "hashed_" + dto.NewPassword;
        tokenRecord.IsRevoked = true;

        _userRepo.Update(tokenRecord.User);
        _dbContext.AuthTokens.Update(tokenRecord);
        await _dbContext.SaveChangesAsync();
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