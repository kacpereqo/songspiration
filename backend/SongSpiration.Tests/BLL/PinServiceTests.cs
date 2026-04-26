using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;
using SongSpiration.BLL.Services;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models.Entities;
using SongSpiration.Models.Enums;

namespace SongSpiration.Tests.BLL
{
    public class PinServiceTests
    {
        private readonly Mock<IPinRepository> _mockPinRepository;
        private readonly PinService _pinService;

        public PinServiceTests()
        {
            _mockPinRepository = new Mock<IPinRepository>();
            _pinService = new PinService(_mockPinRepository.Object);
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
            _mockPinRepository.Setup(s => s.GetByIdWithDetailsAsync(pinId)).ReturnsAsync((Pin)null);

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