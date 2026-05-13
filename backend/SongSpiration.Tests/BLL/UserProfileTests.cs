using Moq;
using SongSpiration.BLL.Services;
using SongSpiration.DAL.Interfaces;
using SongSpiration.BLL.DTOs;
using SongSpiration.Models.Entities;
using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace SongSpiration.Tests.BLL;

public class UserProfileTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPinRepository> _pinRepoMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly SongSpirationDbContext _dbContext;
    private readonly UserService _sut;

    public UserProfileTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _pinRepoMock = new Mock<IPinRepository>();
        _emailSenderMock = new Mock<IEmailSender>();
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(c => c["JwtSettings:Secret"])
            .Returns("super_tajny_klucz_testowy_o_odpowiedniej_dlugosci");

        var options = new DbContextOptionsBuilder<SongSpirationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new SongSpirationDbContext(options);

        _sut = new UserService(
            _userRepoMock.Object, 
            _pinRepoMock.Object, 
            _emailSenderMock.Object, 
            _dbContext,
            _configMock.Object);
    }

    [Fact]
    public async Task GetUserProfileAsync_ValidUser_ReturnsCompleteDtoWithStats()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, DisplayName = "Rockstar", Bio = "I love music" };
        
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _pinRepoMock.Setup(r => r.GetCountByUserIdAsync(userId)).ReturnsAsync(12);
        _pinRepoMock.Setup(r => r.GetTotalLikesReceivedByUserIdAsync(userId)).ReturnsAsync(45);

        var result = await _sut.GetUserProfileAsync(userId);

        Assert.NotNull(result);
        Assert.Equal("Rockstar", result.DisplayName);
        Assert.Equal(12, result.AddedPinsCount);
        Assert.Equal(45, result.TotalLikesReceived);
    }

    [Fact]
    public async Task GetUserProfileAsync_UserNotFound_ReturnsNull()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var result = await _sut.GetUserProfileAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProfileAsync_UserExists_UpdatesDataAndReturnsTrue()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, DisplayName = "Old Name", Bio = "Stary opis" };
        
        var updateDto = new UpdateUserDto 
        { 
            DisplayName = "Test User", 
            Email = "test@example.com",
            Bio = "Mój opis"
        };
    
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
    
        var result = await _sut.UpdateProfileAsync(userId, updateDto);
    
        Assert.True(result);
    
        Assert.Equal("Test User", user.DisplayName); 
        Assert.Equal("Mój opis", user.Bio);          
        
        _userRepoMock.Verify(r => r.Update(user), Times.Once);
    }

    [Fact]
    public async Task DeleteAccountAsync_ValidUser_DeletesAndReturnsTrue()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _sut.DeleteAccountAsync(userId);

        Assert.True(result);
        _userRepoMock.Verify(r => r.Delete(user), Times.Once);
    }
}