using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.Models.Entities;

namespace SongSpiration.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly SongSpirationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IEmailSender emailSender, SongSpirationDbContext dbContext, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            CreatedAt = DateTime.UtcNow,
            Roles = "User"
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = "mock_refresh_token",
            User = MapToDto(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        user.LastLogin = DateTime.UtcNow;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = "mock_refresh_token_" + user.Id,
            User = MapToDto(user)
        };
    }

    public async Task<UserDto?> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<UserDto> UpdateProfileAsync(Guid userId, UserDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        if (updateDto.DisplayName != null)
        {
            user.DisplayName = updateDto.DisplayName;
        }

        if (updateDto.AvatarUrl != null)
        {
            user.AvatarUrl = updateDto.AvatarUrl;
        }

        if (updateDto.Bio != null)
        {
            user.Bio = updateDto.Bio;
        }

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<bool> DeleteAccountAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            // Do not throw an error if the user is not found to prevent user enumeration
            return;
        }

        // Generate reset token
        var resetToken = Guid.NewGuid().ToString("N");
        
        var authToken = new AuthToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = resetToken, // In a real app, this should be hashed, not stored in plain text
            TokenType = TokenType.PasswordReset,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsRevoked = false
        };

        _dbContext.AuthTokens.Add(authToken);
        await _dbContext.SaveChangesAsync();

        // Build reset link (assuming frontend runs on a specific port, normally this is from config)
        var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";

        var message = $@"
            <h3>Password Reset Request</h3>
            <p>You requested a password reset. Click the link below to reset your password:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>If you didn't request this, you can safely ignore this email.</p>";

        await _emailSender.SendEmailAsync(user.Email, "SongSpiration - Password Reset", message);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        // Find valid token
        var tokenRecord = await _dbContext.AuthTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => 
                t.TokenHash == dto.Token && 
                t.TokenType == TokenType.PasswordReset &&
                !t.IsRevoked &&
                t.ExpiryDate > DateTime.UtcNow);

        if (tokenRecord == null)
        {
            throw new InvalidOperationException("Invalid or expired reset token.");
        }

        // Update password
        tokenRecord.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        // Revoke token
        tokenRecord.IsRevoked = true;

        _dbContext.Users.Update(tokenRecord.User);
        _dbContext.AuthTokens.Update(tokenRecord);
        await _dbContext.SaveChangesAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["JwtSettings:Secret"] ?? "TWOJ_BARDZO_DLUGI_I_TAJNY_KLUCZ_MIN_32_ZNAKI";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Roles ?? "User")
        };

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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
