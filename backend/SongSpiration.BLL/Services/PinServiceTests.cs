using Moq;
using SongSpiration.BLL.Services;
using SongSpiration.Models;
using SongSpiration.Models.Dtos;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongSpiration.Tests.Services
{
    public class PinServiceTests
    {
        private readonly Mock<IPinRepository> _pinRepoMock;
        private readonly PinService _pinService;

        public PinServiceTests()
        {
            _pinRepoMock = new Mock<IPinRepository>();
            _pinService = new PinService(_pinRepoMock.Object);
        }

        [Fact]
        public async Task GetFilteredPinsAsync_ShouldSortByLikesDescending()
        {
            // Arrange
            var pins = new List<Pin>
            {
                new Pin { Id = 1, Title = "A", LikeCount = 10, CreatedAt = DateTime.Now },
                new Pin { Id = 2, Title = "B", LikeCount = 50, CreatedAt = DateTime.Now }
            };
            _pinRepoMock.Setup(r => r.GetAllWithGenresAsync()).ReturnsAsync(pins);

            var filter = new PinFilterDto { SortBy = "likes", SortOrder = "desc" };

            // Act
            var result = await _pinService.GetFilteredPinsAsync(filter);

            // Assert
            Assert.Equal(2, result.First().Id); // Pin z 50 lajkami powinien być pierwszy
        }

        [Fact]
        public async Task GetFilteredPinsAsync_ShouldFilterByInstrument()
        {
            // Arrange
            var pins = new List<Pin>
            {
                new Pin { Id = 1, Title = "Guitar Riff", Instrument = Instrument.Guitar },
                new Pin { Id = 2, Title = "Bass Line", Instrument = Instrument.Bass }
            };
            _pinRepoMock.Setup(r => r.GetAllWithGenresAsync()).ReturnsAsync(pins);

            var filter = new PinFilterDto { Instrument = Instrument.Bass };

            // Act
            var result = await _pinService.GetFilteredPinsAsync(filter);

            // Assert
            Assert.Single(result);
            Assert.Equal("Bass Line", result.First().Title);
        }
    }
}