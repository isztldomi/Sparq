using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using System.Text;

namespace Sparq.Tests.UnitTests
{
    public class MediaServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly Mock<IStorageService> _storageMock;
        private readonly MediaService _service;

        public MediaServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);

            _storageMock = new Mock<IStorageService>();

            _service = new MediaService(_context, _storageMock.Object);

            Seed();
        }

        #region Upload

        [Fact]
        public async Task UploadAsync_ShouldCreateMedia()
        {
            // Arrange
            var userId = "user1";

            var fileMock = new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes("test")),
                0,
                4,
                "file",
                "test.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            _storageMock
                .Setup(x => x.UploadAsync(
                    It.IsAny<Stream>(),
                    "test.png",
                    "image/png"))
                .ReturnsAsync("storage-key-123");

            // Act
            var result = await _service.UploadAsync(fileMock, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.OwnerId);
            Assert.Equal("test.png", result.FileName);
            Assert.Equal("storage-key-123", result.StorageKey);
        }

        [Fact]
        public async Task UploadAsync_ShouldThrow_WhenFileEmpty()
        {
            var fileMock = new FormFile(
                new MemoryStream(),
                0,
                0,
                "file",
                "empty.png");

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UploadAsync(fileMock, "user1"));
        }

        #endregion

        #region GetFile (user)

        [Fact]
        public async Task GetFileAsync_ShouldReturnFile_WhenOwnerMatches()
        {
            var media = await _context.Media.FirstAsync();

            _storageMock
                .Setup(x => x.DownloadAsync(media.StorageKey))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes("data")));

            var result = await _service.GetFileAsync(media.Id, media.OwnerId!);

            Assert.NotNull(result.Media);
            Assert.NotNull(result.Stream);
        }

        [Fact]
        public async Task GetFileAsync_ShouldThrow_WhenNotFound()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetFileAsync("invalid", "user1"));
        }

        [Fact]
        public async Task GetFileAsync_ShouldThrowUnauthorized_WhenWrongUser()
        {
            var media = await _context.Media.FirstAsync();

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.GetFileAsync(media.Id, "wrong-user"));
        }

        #endregion

        #region GetFile (public)

        [Fact]
        public async Task GetFileAsync_Public_ShouldReturnFile()
        {
            var media = await _context.Media.FirstAsync();

            _storageMock
                .Setup(x => x.DownloadAsync(media.StorageKey))
                .ReturnsAsync(new MemoryStream());

            var result = await _service.GetFileAsync(media.Id);

            Assert.NotNull(result.Media);
            Assert.NotNull(result.Stream);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteMedia()
        {
            var media = await _context.Media.FirstAsync();

            _storageMock
                .Setup(x => x.DeleteAsync(media.StorageKey))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(media.Id, media.OwnerId!);

            Assert.True(result);

            var db = await _context.Media.FindAsync(media.Id);

            Assert.NotNull(db.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync("invalid", "user1");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenWrongUser()
        {
            var media = await _context.Media.FirstAsync();

            var result = await _service.DeleteAsync(media.Id, "wrong-user");

            Assert.False(result);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var media = new Media
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = "user1",
                FileName = "test.png",
                StorageKey = "key123",
                ContentType = "image/png",
                CreatedAt = DateTime.UtcNow
            };

            _context.Media.Add(media);
            _context.SaveChanges();
        }

        #endregion

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}