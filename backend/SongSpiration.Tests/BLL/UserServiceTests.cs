using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Services;
using SongSpiration.DAL;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models.Entities;
using Xunit;
using SongSpiration.BLL.Interfaces;

namespace SongSpiration.Tests.BLL;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockEmailSender = new Mock<IEmailSender>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup mock configuration for JWT Secret
        _mockConfiguration.Setup(x => x["JwtSettings:Secret"])
            .Returns("TEST_BARDZO_DLUGI_I_TAJNY_KLUCZ_MIN_32_ZNAKI");

        // Pass null for DbContext since Register/Login don't use it directly (they use repository)
        _userService = new UserService(
            _mockUserRepository.Object,
            _mockEmailSender.Object,
            null!, // Assuming DbContext is not used in Register/Login
            _mockConfiguration.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenEmailExists()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            DisplayName = "Test User"
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(registerDto.Email))
            .ReturnsAsync(new User { Email = registerDto.Email });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterAsync(registerDto));
        Assert.Equal("User with this email already exists.", exception.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUserAndReturnToken_WhenDataIsValid()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "newuser@example.com",
            Password = "Password123!",
            DisplayName = "New User"
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.RegisterAsync(registerDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.Equal("newuser@example.com", result.User.Email);
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginAsync(loginDto));
        Assert.Equal("Invalid email or password.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "user@example.com",
            Password = "WrongPassword!"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = loginDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!")
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginAsync(loginDto));
        Assert.Equal("Invalid email or password.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "user@example.com",
            Password = "CorrectPassword!"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = loginDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password),
            Roles = "User"
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.Equal(user.Email, result.User.Email);
        _mockUserRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
