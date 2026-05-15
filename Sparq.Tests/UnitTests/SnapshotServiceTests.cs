using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class SnapshotServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly SnapshotService _snapshotService;

        public SnapshotServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);

            _snapshotService = new SnapshotService(_context);

            SeedDatabase();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateSnapshot()
        {
            // Arrange
            var quiz = await _context.Quizzes.FirstAsync();

            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quiz.Id,
                Title = "New Snapshot",
                Description = "Description",
                TimeLimit = 30
            };

            // Act
            var result = await _snapshotService.CreateAsync(snapshot);

            // Assert
            Assert.NotNull(result);

            var dbSnapshot = await _context.Snapshots
                .FirstOrDefaultAsync(s => s.Id == snapshot.Id);

            Assert.NotNull(dbSnapshot);

            Assert.Equal(2, dbSnapshot.SnapshotNumber);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenQuizNotFound()
        {
            // Arrange
            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = "invalid-id"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _snapshotService.CreateAsync(snapshot));
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSnapshot()
        {
            // Arrange
            var snapshot = await _context.Snapshots.FirstAsync();

            // Act
            var result = await _snapshotService.GetByIdAsync(snapshot.Id);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(snapshot.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenSnapshotNotFound()
        {
            // Act
            var result = await _snapshotService.GetByIdAsync("invalid-id");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSnapshots()
        {
            // Act
            var result = await _snapshotService.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSnapshot()
        {
            // Arrange
            var snapshot = await _context.Snapshots.FirstAsync();

            var updatedSnapshot = new Snapshot
            {
                QuizId = snapshot.QuizId,
                SnapshotNumber = 99,
                Title = "Updated Title",
                Description = "Updated Description",
                TimeLimit = 60,
                Questions = new List<Question>(),
                Sessions = new List<Session>()
            };

            // Act
            var result = await _snapshotService.UpdateAsync(
                snapshot.Id,
                updatedSnapshot);

            // Assert
            Assert.NotNull(result);

            Assert.Equal("Updated Title", result.Title);

            Assert.Equal(60, result.TimeLimit);

            Assert.Equal(99, result.SnapshotNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenSnapshotNotFound()
        {
            // Arrange
            var updatedSnapshot = new Snapshot();

            // Act
            var result = await _snapshotService.UpdateAsync(
                "invalid-id",
                updatedSnapshot);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldDeleteSnapshot()
        {
            // Arrange
            var snapshot = await _context.Snapshots.FirstAsync();

            // Act
            var result = await _snapshotService.DeleteAsync(snapshot.Id);

            // Assert
            Assert.True(result);

            var deletedSnapshot = await _context.Snapshots
                .FindAsync(snapshot.Id);

            Assert.Null(deletedSnapshot);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenSnapshotNotFound()
        {
            // Act
            var result = await _snapshotService.DeleteAsync("invalid-id");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Helpers

        private void SeedDatabase()
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = "user1",
                IsPublic = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quiz.Id,
                Quiz = quiz,
                SnapshotNumber = 1,
                Title = "Initial Snapshot",
                Description = "Initial Description",
                TimeLimit = 30,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>(),
                Sessions = new List<Session>()
            };

            quiz.LastSnapshot = snapshot;

            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);

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