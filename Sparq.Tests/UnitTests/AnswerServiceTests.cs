using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class AnswerServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly AnswerService _service;

        public AnswerServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);
            _service = new AnswerService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldAddAnswer()
        {
            var question = await _context.Questions.FirstAsync();

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                Text = "A",
                IsCorrect = true
            };

            var result = await _service.CreateAsync(answer);

            Assert.NotNull(result);

            var db = await _context.Answers.FindAsync(answer.Id);

            Assert.NotNull(db);
            Assert.Equal("A", db.Text);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_ShouldReturnAnswer()
        {
            var answer = await _context.Answers.FirstAsync();

            var result = await _service.GetByIdAsync(answer.Id);

            Assert.NotNull(result);
            Assert.Equal(answer.Id, result.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenMissing()
        {
            var result = await _service.GetByIdAsync("invalid");

            Assert.Null(result);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ShouldReturnItems()
        {
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_ShouldModifyAnswer()
        {
            var answer = await _context.Answers.FirstAsync();

            var updated = new Answer
            {
                QuestionId = answer.QuestionId,
                Text = "updated",
                IsCorrect = false
            };

            var result = await _service.UpdateAsync(answer.Id, updated);

            Assert.NotNull(result);
            Assert.Equal("updated", result.Text);
            Assert.False(result.IsCorrect);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenMissing()
        {
            var result = await _service.UpdateAsync("invalid", new Answer());

            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ShouldRemoveAnswer()
        {
            var answer = await _context.Answers.FirstAsync();

            var result = await _service.DeleteAsync(answer.Id);

            Assert.True(result);

            var db = await _context.Answers.FindAsync(answer.Id);

            Assert.Null(db);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenMissing()
        {
            var result = await _service.DeleteAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region GetByIds

        [Fact]
        public async Task GetByIds_ShouldReturnMultiple()
        {
            var answers = await _context.Answers.ToListAsync();

            var ids = answers.Select(a => a.Id).ToList();

            var result = await _service.GetByIdsAsync(ids);

            Assert.NotNull(result);
            Assert.Equal(ids.Count, result.Count);
        }

        [Fact]
        public async Task GetByIds_ShouldReturnEmpty_WhenInputEmpty()
        {
            var result = await _service.GetByIdsAsync(new List<string>());

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true
            };

            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quiz.Id,
                Quiz = quiz
            };

            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                SnapshotId = snapshot.Id,
                Snapshot = snapshot
            };

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                Question = question,
                Text = "seed",
                IsCorrect = true
            };

            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);
            _context.Questions.Add(question);
            _context.Answers.Add(answer);

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