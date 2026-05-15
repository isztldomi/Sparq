using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class SessionQuestionStateServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly SessionQuestionStateService _service;

        public SessionQuestionStateServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);

            _service = new SessionQuestionStateService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateState()
        {
            // Arrange
            var question = await _context.Questions.FirstAsync();
            var session = await _context.Sessions.FirstAsync();

            // Act
            var result = await _service.CreateAsync(session.Id, question.Id);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(session.Id, result.SessionId);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenQuestionNotFound()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _service.CreateAsync(session.Id, "invalid");

            Assert.Null(result);
        }

        #endregion

        #region Deactivate

        [Fact]
        public async Task DeactivateCurrentAsync_ShouldDeactivateActiveState()
        {
            var session = await _context.Sessions.FirstAsync();
            var question = await _context.Questions.FirstAsync();

            await _service.CreateAsync(session.Id, question.Id);

            var result = await _service.DeactivateCurrentAsync(session.Id);

            Assert.True(result);

            var state = await _context.SessionQuestionStates.FirstAsync();

            Assert.False(state.IsActive);
        }

        [Fact]
        public async Task DeactivateCurrentAsync_ShouldReturnFalse_WhenNoActiveState()
        {
            var result = await _service.DeactivateCurrentAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region GetActive

        [Fact]
        public async Task GetActiveBySessionIdAsync_ShouldReturnActiveState()
        {
            var session = await _context.Sessions.FirstAsync();
            var question = await _context.Questions.FirstAsync();

            await _service.CreateAsync(session.Id, question.Id);

            var result = await _service.GetActiveBySessionIdAsync(session.Id);

            Assert.NotNull(result);
            Assert.True(result.IsActive);
            Assert.Equal(session.Id, result.SessionId);
        }

        [Fact]
        public async Task GetActiveBySessionIdAsync_ShouldReturnNull_WhenNoActiveState()
        {
            var result = await _service.GetActiveBySessionIdAsync("invalid");

            Assert.Null(result);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                IsPublic = true,
                IsActive = true
            };

            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quiz.Id,
                Quiz = quiz
            };

            var session = new Session
            {
                Id = Guid.NewGuid().ToString(),
                SnapshotId = snapshot.Id,
                Snapshot = snapshot
            };

            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Order = 1,
                TimeLimit = 30,
                SnapshotId = snapshot.Id
            };

            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);
            _context.Sessions.Add(session);
            _context.Questions.Add(question);

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