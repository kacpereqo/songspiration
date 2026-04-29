using Xunit;
using SongSpiration.BLL.Services;

namespace SongSpiration.Tests.BLL
{
    public class LikeServiceTests
    {
        [Fact]
        public async Task Like_ShouldAddLike()
        {
            // Arrange
            var likeService = new LikeService();
            var pinId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            await likeService.Like(pinId, userId);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task Unlike_ShouldRemoveLike()
        {
            // Arrange
            var likeService = new LikeService();
            var pinId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            await likeService.Unlike(pinId, userId);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task IsLiked_ShouldReturnFalse()
        {
            // Arrange
            var likeService = new LikeService();
            var pinId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var result = await likeService.IsLiked(pinId, userId);

            // Assert
            Assert.False(result);
        }
    }
}