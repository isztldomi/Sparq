using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class ParticipantServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly ParticipantService _service;

        public ParticipantServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);
            _service = new ParticipantService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldInitializeDefaults()
        {
            var session = await _context.Sessions.FirstAsync();
            var user = await _context.Users.FirstAsync();

            var participant = new Participant
            {
                Id = Guid.NewGuid().ToString(),
                SessionId = session.Id,
                UserId = user.Id
            };

            var result = await _service.CreateAsync(participant);

            Assert.NotNull(result);
            Assert.Equal(0, result.Score);
            Assert.Equal(0, result.Rank);
            Assert.False(result.IsFinished);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnParticipant()
        {
            var participant = await _context.Participants.FirstAsync();

            var result = await _service.GetByIdAsync(participant.Id);

            Assert.NotNull(result);
            Assert.Equal(participant.Id, result.Id);
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
        public async Task GetAllAsync_ShouldReturnParticipants()
        {
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateParticipant()
        {
            var participant = await _context.Participants.FirstAsync();

            var updated = new Participant
            {
                UserId = participant.UserId,
                SessionId = participant.SessionId,
                DisplayName = "new-name",
                Score = 100,
                Rank = 1,
                IsFinished = true,
                ParticipantAnswers = new List<ParticipantAnswer>(),
                Messages = new List<Message>()
            };

            var result = await _service.UpdateAsync(participant.Id, updated);

            Assert.NotNull(result);
            Assert.Equal("new-name", result.DisplayName);
            Assert.Equal(100, result.Score);
            Assert.True(result.IsFinished);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.UpdateAsync("invalid", new Participant());

            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldRemoveParticipant()
        {
            var participant = await _context.Participants.FirstAsync();

            var result = await _service.DeleteAsync(participant.Id);

            Assert.True(result);

            var db = await _context.Participants.FindAsync(participant.Id);

            Assert.Null(db);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region Session queries

        [Fact]
        public async Task GetBySessionId_ShouldReturnParticipants()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _service.GetBySessionIdAsync(session.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllParticipantsBySessionId_ShouldReturnParticipants()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _service.GetAllParticipantsBySessionIdAsync(session.Id);

            Assert.NotNull(result);
        }

        #endregion

        #region Exists checks

        [Fact]
        public async Task IsUserJoined_ShouldReturnTrue_WhenExists()
        {
            var p = await _context.Participants.FirstAsync();

            var result = await _service.IsUserJoinedAsync(p.UserId!, p.SessionId!);

            Assert.True(result);
        }

        [Fact]
        public async Task IsExtUserJoined_ShouldReturnFalse_WhenNotExists()
        {
            var result = await _service.IsExtUserJoinedAsync("ext", "invalid");

            Assert.False(result);
        }

        #endregion

        #region Get by composite keys

        [Fact]
        public async Task GetByUserIdAndSession_ShouldReturnParticipant()
        {
            var p = await _context.Participants.FirstAsync();

            var result = await _service
                .GetIdByUserIdAndSessionIdAsync(p.UserId!, p.SessionId!);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByExtUserIdAndSession_ShouldReturnNull_WhenMissing()
        {
            var result = await _service
                .GetIdByExtUserIdAndSessionIdAsync("ext", "invalid");

            Assert.Null(result);
        }

        #endregion

        #region Query

        [Fact]
        public void GetUserSessionQuery_ShouldReturnQueryable()
        {
            var p = _context.Participants.First();

            var query = _service.GetUserSessionQuery(p.UserId!);

            Assert.NotNull(query);
        }

        #endregion

        #region Seed

        private void Seed()
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.com"
            };

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

            var participant = new Participant
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                User = user,
                SessionId = session.Id,
                Session = session,
                Score = 5,
                Rank = 1,
                IsFinished = false
            };

            _context.Users.Add(user);
            _context.Quizzes.Add(quiz);
            _context.Snapshots.Add(snapshot);
            _context.Sessions.Add(session);
            _context.Participants.Add(participant);

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