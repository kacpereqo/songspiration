using Moq;
using SongSpiration.BLL.Services;
using SongSpiration.DAL.Interfaces;
using SongSpiration.BLL.DTOs;
using SongSpiration.Models.Entities;
using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL; // Dodane dla DbContext
using Microsoft.EntityFrameworkCore; // Dodane dla opcji DbContext
using Xunit;

namespace SongSpiration.Tests.BLL;

public class UserProfileTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPinRepository> _pinRepoMock;
    private readonly Mock<IEmailSender> _emailSenderMock; // 1. Dodany mock mailera
    private readonly SongSpirationDbContext _dbContext;   // 2. Potrzebny kontekst bazy
    private readonly UserService _sut;

    public UserProfileTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _pinRepoMock = new Mock<IPinRepository>();
        _emailSenderMock = new Mock<IEmailSender>(); // Inicjalizacja mocka

        // 3. Tworzymy "pusty" DbContext w pamięci (InMemory), aby serwis mógł się zainicjalizować
        var options = new DbContextOptionsBuilder<SongSpirationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new SongSpirationDbContext(options);

        // 4. Przekazujemy wszystkie 4 wymagane parametry do konstruktora
        _sut = new UserService(
            _userRepoMock.Object, 
            _pinRepoMock.Object, 
            _emailSenderMock.Object, 
            _dbContext);
    }

    [Fact]
    public async Task GetUserProfileAsync_ValidUser_ReturnsCompleteDtoWithStats()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, DisplayName = "Rockstar", Bio = "I love music" };
        
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _pinRepoMock.Setup(r => r.GetCountByUserIdAsync(userId)).ReturnsAsync(12);
        _pinRepoMock.Setup(r => r.GetTotalLikesReceivedByUserIdAsync(userId)).ReturnsAsync(45);

        // Act
        var result = await _sut.GetUserProfileAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rockstar", result.DisplayName);
        Assert.Equal(12, result.AddedPinsCount);
        Assert.Equal(45, result.TotalLikesReceived);
    }

    [Fact]
    public async Task GetUserProfileAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        // Act
        var result = await _sut.GetUserProfileAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProfileAsync_UserExists_UpdatesDataAndReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, DisplayName = "Old Name" };
        var updateDto = new UpdateUserDto { DisplayName = "New Name", Bio = "New Bio" };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateProfileAsync(userId, updateDto);

        // Assert
        Assert.True(result);
        Assert.Equal("New Name", user.DisplayName);
        Assert.Equal("New Bio", user.Bio);
        _userRepoMock.Verify(r => r.Update(user), Times.Once);
    }

    [Fact]
    public async Task DeleteAccountAsync_ValidUser_DeletesAndReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _sut.DeleteAccountAsync(userId);

        // Assert
        Assert.True(result);
        _userRepoMock.Verify(r => r.Delete(user), Times.Once);
    }
}