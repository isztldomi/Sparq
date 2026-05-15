using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class QuestionServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly QuestionService _service;

        public QuestionServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);
            _service = new QuestionService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateQuestion()
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Q1"
            };

            var result = await _service.CreateAsync(question);

            Assert.NotNull(result);

            var db = await _context.Questions.FindAsync(question.Id);

            Assert.NotNull(db);
            Assert.Equal("Q1", db.Title);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuestion()
        {
            var question = await _context.Questions.FirstAsync();

            var result = await _service.GetByIdAsync(question.Id);

            Assert.NotNull(result);
            Assert.Equal(question.Id, result.Id);
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
        public async Task GetAllAsync_ShouldReturnQuestions()
        {
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateQuestion()
        {
            var question = await _context.Questions.FirstAsync();

            var updated = new Question
            {
                SnapshotId = question.SnapshotId,
                Title = "Updated",
                Text = "New text",
                TimeLimit = 50,
                Point = 10,
                Answers = new List<Answer>(),
                ParticipantAnswers = new List<ParticipantAnswer>(),
                Messages = new List<Message>()
            };

            var result = await _service.UpdateAsync(question.Id, updated);

            Assert.NotNull(result);
            Assert.Equal("Updated", result.Title);
            Assert.Equal(50, result.TimeLimit);
            Assert.Equal(10, result.Point);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.UpdateAsync("invalid", new Question());

            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldDeleteQuestion()
        {
            var question = await _context.Questions.FirstAsync();

            var result = await _service.DeleteAsync(question.Id);

            Assert.True(result);

            var db = await _context.Questions.FindAsync(question.Id);

            Assert.Null(db);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region GetBySessionIdAndOrder

        [Fact]
        public async Task GetBySessionIdAndOrder_ShouldReturnQuestion()
        {
            var session = await _context.Sessions.FirstAsync();
            var question = await _context.Questions.FirstAsync();

            question.Order = 1;

            var snapshot = await _context.Snapshots.FindAsync(question.SnapshotId);
            snapshot!.Sessions = new List<Session> { session };

            await _context.SaveChangesAsync();

            var result = await _service.GetBySessionIdAndOrderAsync(session.Id, 1);

            Assert.NotNull(result);
            Assert.Equal(question.Id, result.Id);
        }

        [Fact]
        public async Task GetBySessionIdAndOrder_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.GetBySessionIdAndOrderAsync("invalid", 1);

            Assert.Null(result);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                IsPublic = true
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
                SnapshotId = snapshot.Id,
                Snapshot = snapshot,
                Title = "Q1",
                Order = 1
            };

            snapshot.Sessions = new List<Session> { session };

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