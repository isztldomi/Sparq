using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;

namespace Sparq.Tests.UnitTests
{
    public class SessionServiceTests : IDisposable
    {
        private readonly SparqDbContext _context;
        private readonly SessionService _sessionService;

        public SessionServiceTests()
        {
            var options = new DbContextOptionsBuilder<SparqDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SparqDbContext(options);

            _sessionService = new SessionService(_context);

            Seed();
        }

        #region Create

        [Fact]
        public async Task CreateAsync_ShouldCreateSession()
        {
            var snapshot = await _context.Snapshots.FirstAsync();

            var result = await _sessionService.CreateAsync(snapshot.Id);

            Assert.NotNull(result);
            Assert.Equal(snapshot.Id, result.SnapshotId);
            Assert.Equal(snapshot.PinCode, result.PinCode);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenSnapshotNotFound()
        {
            var result = await _sessionService.CreateAsync("invalid");

            Assert.Null(result);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSession()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _sessionService.GetByIdAsync(session.Id);

            Assert.NotNull(result);
            Assert.Equal(session.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _sessionService.GetByIdAsync("invalid");

            Assert.Null(result);
        }

        #endregion

        #region Exists

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _sessionService.ExistsAsync(session.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
        {
            var result = await _sessionService.ExistsAsync("invalid");

            Assert.False(result);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAllAsync_ShouldReturnSessions()
        {
            var result = await _sessionService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSession()
        {
            var session = await _context.Sessions.FirstAsync();

            session.Status = SessionStatus.Created;

            var updated = new Session
            {
                SnapshotId = session.SnapshotId,
                Status = SessionStatus.Running,
                PinCode = "9999",
                Participants = new List<Participant>(),
                Messages = new List<Message>()
            };

            var result = await _sessionService.UpdateAsync(session.Id, updated);

            Assert.NotNull(result);
            Assert.Equal(SessionStatus.Running, result.Status);
            Assert.Equal("9999", result.PinCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotFound()
        {
            var updated = new Session();

            var result = await _sessionService.UpdateAsync("invalid", updated);

            Assert.Null(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteAsync_ShouldDelete_WhenStatusCreated()
        {
            var session = await _context.Sessions.FirstAsync();
            session.Status = SessionStatus.Created;
            await _context.SaveChangesAsync();

            var result = await _sessionService.DeleteAsync(session.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldFail_WhenNotCreated()
        {
            var session = await _context.Sessions.FirstAsync();
            session.Status = SessionStatus.Running;
            await _context.SaveChangesAsync();

            var result = await _sessionService.DeleteAsync(session.Id);

            Assert.False(result);
        }

        #endregion

        #region Activate

        [Fact]
        public async Task Activate_ShouldSetWaiting()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _sessionService.ActivateForWaitingByIdAsync(session.Id);

            Assert.True(result);

            var updated = await _context.Sessions.FindAsync(session.Id);

            Assert.Equal(SessionStatus.Waiting, updated!.Status);
        }

        #endregion

        #region Start

        [Fact]
        public async Task Start_ShouldFail_WhenNoParticipants()
        {
            var session = await _context.Sessions.FirstAsync();

            session.Participants.Clear();
            await _context.SaveChangesAsync();

            var result = await _sessionService.StartSessionAsync(session.Id);

            Assert.False(result);
        }

        [Fact]
        public async Task Start_ShouldWork_WhenParticipantsExist()
        {
            var session = await _context.Sessions.FirstAsync();

            session.Participants.Add(new Participant
            {
                Id = Guid.NewGuid().ToString()
            });

            await _context.SaveChangesAsync();

            var result = await _sessionService.StartSessionAsync(session.Id);

            Assert.True(result);

            var updated = await _context.Sessions.FindAsync(session.Id);

            Assert.Equal(SessionStatus.Running, updated!.Status);
        }

        #endregion

        #region End

        [Fact]
        public async Task End_ShouldFinishSession()
        {
            var session = await _context.Sessions.FirstAsync();

            var result = await _sessionService.EndSessionAsync(session.Id);

            Assert.True(result);

            var updated = await _context.Sessions.FindAsync(session.Id);

            Assert.Equal(SessionStatus.Finished, updated!.Status);
        }

        #endregion

        #region Paged

        [Fact]
        public async Task GetAllPublicWaitingSessionsPaged_ShouldReturnResult()
        {
            var result = await _sessionService
                .GetAllPublicWaitingSessionsPagedAsync(1, 10);

            Assert.NotNull(result.Items);
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
                Quiz = quiz,
                PinCode = "1234"
            };

            var session = new Session
            {
                Id = Guid.NewGuid().ToString(),
                SnapshotId = snapshot.Id,
                Snapshot = snapshot,
                Status = SessionStatus.Created,
                Participants = new List<Participant>(),
                Messages = new List<Message>()
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