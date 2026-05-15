using Sparq.DataAccess.Services;
using System.Text;

namespace Sparq.Tests.UnitTests
{
    public class LocalStorageServiceTests : IDisposable
    {
        private readonly LocalStorageService _service;
        private readonly string _testDir;

        public LocalStorageServiceTests()
        {
            // Átmeneti working directory
            _testDir = Path.Combine(Directory.GetCurrentDirectory(), "storage");

            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);

            _service = new LocalStorageService();
        }

        #region Upload

        [Fact]
        public async Task UploadAsync_ShouldCreateFile_AndReturnKey()
        {
            // Arrange
            var content = Encoding.UTF8.GetBytes("hello world");
            using var stream = new MemoryStream(content);

            // Act
            var key = await _service.UploadAsync(stream, "test.txt", "text/plain");

            // Assert
            Assert.NotNull(key);

            var path = Path.Combine(_testDir, key);
            Assert.True(File.Exists(path));
        }

        #endregion

        #region Download

        [Fact]
        public async Task DownloadAsync_ShouldReturnStream_WhenFileExists()
        {
            var content = Encoding.UTF8.GetBytes("data");
            using var uploadStream = new MemoryStream(content);

            var key = await _service.UploadAsync(uploadStream, "file.txt", "text/plain");

            var resultStream = await _service.DownloadAsync(key);

            using var reader = new StreamReader(resultStream);
            var result = await reader.ReadToEndAsync();

            Assert.Equal("data", result);
        }

        [Fact]
        public async Task DownloadAsync_ShouldThrow_WhenFileMissing()
        {
            await Assert.ThrowsAsync<FileNotFoundException>(() =>
                _service.DownloadAsync("invalid-key"));
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldRemoveFile()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("delete me"));

            var key = await _service.UploadAsync(stream, "del.txt", "text/plain");

            await _service.DeleteAsync(key);

            var path = Path.Combine(_testDir, key);

            Assert.False(File.Exists(path));
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenFileMissing()
        {
            await _service.DeleteAsync("non-existent-key");

            Assert.True(true); // ha idáig eljut, ok
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }

        #endregion
    }
}