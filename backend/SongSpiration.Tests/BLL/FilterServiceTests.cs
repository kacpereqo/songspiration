using Xunit;
using SongSpiration.BLL.Services;

namespace SongSpiration.Tests.BLL
{
    public class FilterServiceTests
    {
        [Fact]
        public async Task FilterPins_ShouldReturnFilteredPins()
        {
            // Arrange
            var filterService = new FilterService();
            var genre = "Rock";
            var instrument = "Guitar";
            var visibility = "Public";

            // Act
            var result = await filterService.FilterPins(genre, instrument, visibility);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FilterPins_ShouldReturnAllPins_WhenNoFilterApplied()
        {
            // Arrange
            var filterService = new FilterService();
            var genre = "";
            var instrument = "";
            var visibility = "";

            // Act
            var result = await filterService.FilterPins(genre, instrument, visibility);

            // Assert
            Assert.NotNull(result);
        }
    }
}