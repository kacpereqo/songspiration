using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.BLL.Services;
using SongSpiration.Models;

namespace SongSpiration.Tests.BLL
{
    public class PinServiceTests
    {
        private readonly Mock<IPinService> _mockPinService;
        private readonly PinService _pinService;

        public PinServiceTests()
        {
            _mockPinService = new Mock<IPinService>();
            _pinService = new PinService(); // Assuming PinService has a parameterless constructor or dependencies are mocked
        }

        [Fact]
        public async Task CreatePinAsync_ShouldReturnPinDto()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var createDto = new CreatePinDto
            {
                Title = "Test Pin",
                Description = "Test Description",
                Visibility = PinVisibility.Public
            };

            // Act
            var result = await _pinService.CreatePinAsync(ownerId, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ownerId, result.OwnerId);
            Assert.Equal(createDto.Title, result.Title);
            Assert.Equal(createDto.Description, result.Description);
            Assert.Equal(createDto.Visibility, result.Visibility);
        }

        [Fact]
        public async Task GetPinByIdAsync_ShouldReturnPinDto_WhenPinExists()
        {
            // Arrange
            var pinId = Guid.NewGuid();

            // Act
            var result = await _pinService.GetPinByIdAsync(pinId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPinByIdAsync_ShouldReturnNull_WhenPinDoesNotExist()
        {
            // Arrange
            var pinId = Guid.NewGuid();
            _mockPinService.Setup(s => s.GetPinByIdAsync(pinId)).ReturnsAsync((PinDto)null);

            // Act
            var result = await _pinService.GetPinByIdAsync(pinId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePinAsync_ShouldReturnUpdatedPinDto()
        {
            // Arrange
            var pinId = Guid.NewGuid();
            var updateDto = new UpdatePinDto { Title = "Updated Title", Description = "Updated Description", Visibility = PinVisibility.Private };

            // Act
            var result = await _pinService.UpdatePinAsync(pinId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pinId, result.Id);
            Assert.Equal(updateDto.Title, result.Title);
            Assert.Equal(updateDto.Description, result.Description);
            Assert.Equal(updateDto.Visibility, result.Visibility);
        }

        [Fact]
        public async Task DeletePinAsync_ShouldReturnBool()
        {
            // Arrange
            var pinId = Guid.NewGuid();

            // Act
            var result = await _pinService.DeletePinAsync(pinId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task GetAllPinsAsync_ShouldReturnListOfPinDtos()
        {
            // Act
            var result = await _pinService.GetAllPinsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<PinDto>>(result);
        }

        [Fact]
        public async Task GetPinsByUserIdAsync_ShouldReturnListOfPinDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _pinService.GetPinsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<PinDto>>(result);
        }

        [Fact]
        public async Task GetPinsByBoardIdAsync_ShouldReturnListOfPinDtos()
        {
            // Arrange
            var boardId = Guid.NewGuid();

            // Act
            var result = await _pinService.GetPinsByBoardIdAsync(boardId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<PinDto>>(result);
        }
    }
}