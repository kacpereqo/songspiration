using Moq;
using SongSpiration.BLL.Interfaces;
using SongSpiration.BLL.Services;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.BLL.DTOs;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.Tests.BLL
{
    public class AdminPanelServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IPinRepository> _pinRepositoryMock;
        private readonly IAdminPanelService _adminPanelService;

        public AdminPanelServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _pinRepositoryMock = new Mock<IPinRepository>();
            _adminPanelService = new AdminPanelService(_userRepositoryMock.Object, _genreRepositoryMock.Object, _pinRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, Username = "testuser", Email = "testuser@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _adminPanelService.GetUserByIdAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            User? expectedUser = null;
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _adminPanelService.GetUserByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await _adminPanelService.GetAllUsersAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                PasswordHash = "hashedpassword",
                Role = "User"
            };
            var createdUser = new User
            {
                Id = 1,
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.PasswordHash,
                Role = userCreateDto.Role
            };
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _adminPanelService.CreateUserAsync(userCreateDto);

            // Assert
            result.Should().BeEquivalentTo(createdUser);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var userUpdateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updateduser@example.com",
                PasswordHash = "newhashedpassword",
                Role = "Admin"
            };
            var existingUser = new User
            {
                Id = userId,
                Username = "olduser",
                Email = "olduser@example.com",
                PasswordHash = "oldhashedpassword",
                Role = "User"
            };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.UpdateUserAsync(userId, userUpdateDto);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var userUpdateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updateduser@example.com",
                PasswordHash = "newhashedpassword",
                Role = "Admin"
            };
            User? existingUser = null;
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _adminPanelService.UpdateUserAsync(userId, userUpdateDto));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User
            {
                Id = userId,
                Username = "olduser",
                Email = "olduser@example.com",
                PasswordHash = "oldhashedpassword",
                Role = "User"
            };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.Delete(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            User? existingUser = null;
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _adminPanelService.DeleteUserAsync(userId));
        }

        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnAllGenres()
        {
            // Arrange
            var expectedGenres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Genre1" },
                new Genre { Id = 2, Name = "Genre2" }
            };
            _genreRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedGenres);

            // Act
            var result = await _adminPanelService.GetAllGenresAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedGenres);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnGenre_WhenGenreExists()
        {
            // Arrange
            var genreId = 1;
            var expectedGenre = new Genre { Id = genreId, Name = "Genre1" };
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(expectedGenre);

            // Act
            var result = await _adminPanelService.GetGenreByIdAsync(genreId);

            // Assert
            result.Should().BeEquivalentTo(expectedGenre);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnNull_WhenGenreDoesNotExist()
        {
            // Arrange
            var genreId = 1;
            Genre? expectedGenre = null;
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(expectedGenre);

            // Act
            var result = await _adminPanelService.GetGenreByIdAsync(genreId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldCreateGenre()
        {
            // Arrange
            var genreCreateDto = new GenreCreateDto { Name = "NewGenre" };
            var createdGenre = new Genre { Id = 1, Name = genreCreateDto.Name };
            _genreRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Genre>())).Returns(Task.CompletedTask);
            _genreRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _adminPanelService.CreateGenreAsync(genreCreateDto);

            // Assert
            result.Should().BeEquivalentTo(createdGenre);
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldUpdateGenre_WhenGenreExists()
        {
            // Arrange
            var genreId = 1;
            var genreUpdateDto = new GenreCreateDto { Name = "UpdatedGenre" };
            var existingGenre = new Genre { Id = genreId, Name = "OldGenre" };
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(existingGenre);
            _genreRepositoryMock.Setup(repo => repo.Update(It.IsAny<Genre>())).Verifiable();
            _genreRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.UpdateGenreAsync(genreId, genreUpdateDto);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.Update(It.IsAny<Genre>()), Times.Once);
            _genreRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldThrowException_WhenGenreDoesNotExist()
        {
            // Arrange
            var genreId = 1;
            var genreUpdateDto = new GenreCreateDto { Name = "UpdatedGenre" };
            Genre? existingGenre = null;
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(existingGenre);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _adminPanelService.UpdateGenreAsync(genreId, genreUpdateDto));
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldDeleteGenre_WhenGenreExists()
        {
            // Arrange
            var genreId = 1;
            var existingGenre = new Genre { Id = genreId, Name = "OldGenre" };
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(existingGenre);
            _genreRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Genre>())).Verifiable();
            _genreRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.DeleteGenreAsync(genreId);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Genre>()), Times.Once);
            _genreRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldThrowException_WhenGenreDoesNotExist()
        {
            // Arrange
            var genreId = 1;
            Genre? existingGenre = null;
            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(existingGenre);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _adminPanelService.DeleteGenreAsync(genreId));
        }

        [Fact]
        public async Task SearchUsersAsync_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var searchTerm = "test";
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Username = "testuser1", Email = "testuser1@example.com" },
                new User { Id = 2, Username = "testuser2", Email = "testuser2@example.com" }
            };
            _userRepositoryMock.Setup(repo => repo.SearchAsync(searchTerm)).ReturnsAsync(expectedUsers);

            // Act
            var result = await _adminPanelService.SearchUsersAsync(searchTerm);

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task SearchUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var searchTerm = "test";
            var expectedUsers = new List<User>();
            _userRepositoryMock.Setup(repo => repo.SearchAsync(searchTerm)).ReturnsAsync(expectedUsers);

            // Act
            var result = await _adminPanelService.SearchUsersAsync(searchTerm);

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task BanUserAsync_ShouldBanUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User { Id = userId, Username = "testuser", Email = "testuser@example.com", IsBanned = false };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.BanUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task BanUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            User? existingUser = null;
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _adminPanelService.BanUserAsync(userId));
        }

        [Fact]
        public async Task DeletePinsForUserAsync_ShouldDeletePins_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var pins = new List<Pin>
            {
                new Pin { Id = 1, UserId = userId },
                new Pin { Id = 2, UserId = userId }
            };
            _pinRepositoryMock.Setup(repo => repo.GetPinsByUserIdAsync(userId)).ReturnsAsync(pins);
            _pinRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Pin>())).Verifiable();
            _pinRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _adminPanelService.DeletePinsForUserAsync(userId);

            // Assert
            _pinRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Pin>()), Times.Exactly(pins.Count));
            _pinRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}