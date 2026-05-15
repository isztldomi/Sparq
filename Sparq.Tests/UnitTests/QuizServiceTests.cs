using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class QuizServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly QuizService _service;

        public QuizServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);
            _service = new QuizService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateQuizWithSnapshot()
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                Snapshots = new List<Snapshot>
                {
                    new Snapshot
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            var result = await _service.CreateAsync(quiz);

            Assert.NotNull(result);
            Assert.True(result.IsActive);
            Assert.NotNull(result.LastSnapshotId);

            var dbQuiz = await _context.Quizzes
                .Include(q => q.Snapshots)
                .FirstOrDefaultAsync(q => q.Id == quiz.Id);

            Assert.NotNull(dbQuiz);
            Assert.Single(dbQuiz.Snapshots);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuiz()
        {
            var quiz = await _context.Quizzes.FirstAsync();

            var result = await _service.GetByIdAsync(quiz.Id);

            Assert.NotNull(result);
            Assert.Equal(quiz.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.GetByIdAsync("invalid");

            Assert.Null(result);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuizzes()
        {
            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateQuiz()
        {
            var quiz = await _context.Quizzes.FirstAsync();

            var updated = new Quiz
            {
                OwnerId = "new-owner",
                IsPublic = false,
                IsActive = true,
                Snapshots = new List<Snapshot>()
            };

            var result = await _service.UpdateAsync(quiz.Id, updated);

            Assert.NotNull(result);
            Assert.Equal("new-owner", result.OwnerId);
            Assert.False(result.IsPublic);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.UpdateAsync("invalid", new Quiz());

            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldDeleteQuiz()
        {
            var quiz = await _context.Quizzes.FirstAsync();

            var result = await _service.DeleteAsync(quiz.Id);

            Assert.True(result);

            var db = await _context.Quizzes.FindAsync(quiz.Id);

            Assert.Null(db);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region Deactivate

        [Fact]
        public async Task DeactivateAsync_ShouldDeactivateQuiz()
        {
            var quiz = await _context.Quizzes.FirstAsync();

            await _service.DeactivateAsync(quiz.Id, "user1");

            var db = await _context.Quizzes.FindAsync(quiz.Id);

            Assert.False(db!.IsActive);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldThrow_WhenNotFound()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.DeactivateAsync("invalid", "user1"));
        }

        #endregion

        #region GetByUserPaged

        [Fact]
        public async Task GetByUserPagedAsync_ShouldReturnPaged()
        {
            var result = await _service.GetByUserPagedAsync("user1", 1, 10);

            Assert.True(result.TotalCount > 0);
            Assert.NotNull(result.Items);
        }

        #endregion

        #region GetQuizSessionsPaged

        [Fact]
        public async Task GetQuizSessionsPagedAsync_ShouldReturnSessions()
        {
            var quiz = await _context.Quizzes.FirstAsync();

            var result = await _service.GetQuizSessionsPagedAsync(
                quiz.Id, 1, 10);

            Assert.NotNull(result.Items);
        }

        #endregion

        #region Seed

        private void Seed()
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
                SnapshotNumber = 1
            };

            var session = new Session
            {
                Id = Guid.NewGuid().ToString(),
                SnapshotId = snapshot.Id,
                Snapshot = snapshot,
                CreatedAt = DateTime.UtcNow
            };

            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);
            _context.Sessions.Add(session);

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