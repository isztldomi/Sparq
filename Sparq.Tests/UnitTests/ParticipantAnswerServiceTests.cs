using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class ParticipantAnswerServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly ParticipantAnswerService _service;

        public ParticipantAnswerServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);
            _service = new ParticipantAnswerService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldSetTimestampAndScoreCorrectly()
        {
            var participant = await _context.Participants.FirstAsync();
            var question = await _context.Questions.FirstAsync();
            var answer = await _context.Answers.FirstAsync();

            var entity = new ParticipantAnswer
            {
                Id = Guid.NewGuid().ToString(),
                ParticipantId = participant.Id,
                QuestionId = question.Id,
                AnswerId = answer.Id,
                Answer = answer
            };

            var result = await _service.CreateAsync(entity);

            Assert.NotNull(result);
            Assert.NotNull(result.AnsweredAt);

            if (answer.IsCorrect)
                Assert.Equal(1, result.PointsEarned);
            else
                Assert.Equal(0, result.PointsEarned);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetByIdAsync(pa.Id);

            Assert.NotNull(result);
            Assert.Equal(pa.Id, result.Id);
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
        public async Task GetAllAsync_ShouldReturnItems()
        {
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.DeleteAsync(pa.Id);

            Assert.True(result);

            var db = await _context.ParticipantAnswers.FindAsync(pa.Id);

            Assert.Null(db);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region GetByParticipant

        [Fact]
        public async Task GetByParticipantId_ShouldReturnItems()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetByParticipantIdAsync(pa.ParticipantId!);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region GetByQuestion

        [Fact]
        public async Task GetByQuestionId_ShouldReturnItems()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetByQuestionIdAsync(pa.QuestionId!);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region GetBySessionAndQuestion

        [Fact]
        public async Task GetBySessionAndQuestion_ShouldReturnItems()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service
                .GetBySessionAndQuestionAsync(pa.SessionId!, pa.QuestionId!);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region GetBySession

        [Fact]
        public async Task GetBySession_ShouldReturnItems()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetBySessionAsync(pa.SessionId!);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region GetParticipantAnswer (complex filter)

        [Fact]
        public async Task GetParticipantAnswer_ShouldReturnByUserId()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetParticipantAnswerAsync(
                pa.SessionId!,
                pa.QuestionId!,
                pa.Participant!.UserId,
                null);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetParticipantAnswer_ShouldReturnNull_WhenNoIdentifiers()
        {
            var pa = await _context.ParticipantAnswers.FirstAsync();

            var result = await _service.GetParticipantAnswerAsync(
                pa.SessionId!,
                pa.QuestionId!,
                null,
                null);

            Assert.Null(result);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var user = new User { Id = Guid.NewGuid().ToString() };

            var quiz = new Quiz { Id = Guid.NewGuid().ToString() };

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

            var participant = new Participant
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                User = user,
                SessionId = session.Id,
                Session = session
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
                IsCorrect = true
            };

            var participantAnswer = new ParticipantAnswer
            {
                Id = Guid.NewGuid().ToString(),
                ParticipantId = participant.Id,
                Participant = participant,
                QuestionId = question.Id,
                Question = question,
                AnswerId = answer.Id,
                Answer = answer,
                SessionId = session.Id
            };

            _context.Users.Add(user);
            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);
            _context.Sessions.Add(session);
            _context.Participants.Add(participant);
            _context.Questions.Add(question);
            _context.Answers.Add(answer);
            _context.ParticipantAnswers.Add(participantAnswer);

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