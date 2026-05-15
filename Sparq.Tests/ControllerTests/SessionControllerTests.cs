using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.WebApi.Controllers;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Services;

namespace Sparq.Tests.ControllerTests;

public class SessionControllerTests : IDisposable
{
    private readonly SparqDbContext _context;
    private readonly SessionController _controller;

    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<IQuizService> _quizServiceMock = new();
    private readonly Mock<ISnapshotService> _snapshotServiceMock = new();
    private readonly Mock<ISessionService> _sessionServiceMock = new();
    private readonly Mock<IParticipantService> _participantServiceMock = new();
    private readonly Mock<IQuestionService> _questionServiceMock = new();
    private readonly Mock<ISessionQuestionStateService> _stateServiceMock = new();
    private readonly Mock<IParticipantAnswerService> _answerServiceMock = new();
    private readonly Mock<ISessionsNotificationService> _notificationMock = new();

    public SessionControllerTests()
    {
        var options = new DbContextOptionsBuilder<SparqDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SparqDbContext(options);

        _controller = new SessionController(
            mapper: null!,
            sessionService: _sessionServiceMock.Object,
            usersService: _usersServiceMock.Object,
            quizService: _quizServiceMock.Object,
            snapshotService: _snapshotServiceMock.Object,
            participantService: _participantServiceMock.Object,
            questionService: _questionServiceMock.Object,
            sessionQuestionStateService: _stateServiceMock.Object,
            participantAnswerService: _answerServiceMock.Object,
            sessionsNotificationService: _notificationMock.Object
        );

        Seed();
    }

    private void Seed()
    {
        _context.Users.Add(new User
        {
            Id = "user-1",
            Email = "test@test.com",
            UserName = "test"
        });

        _context.Quizzes.Add(new Quiz
        {
            Id = "quiz-1",
            OwnerId = "user-1",
            IsActive = true,
            IsPublic = true,
            LastSnapshot = new Snapshot { Id = "snap-1" }
        });

        _context.SaveChanges();
    }

    private void SetUser(string? userId)
    {
        _usersServiceMock
            .Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(userId == null
                ? null
                : new User { Id = userId });
    }

    // CREATE SESSION
    [Fact]
    public async Task Create_ReturnsUnauthorized_WhenNoUser()
    {
        SetUser(null);

        var result = await _controller.Create(new CreateSessionRequestDto
        {
            QuizId = "quiz-1"
        });

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsForbid_WhenNotOwner()
    {
        SetUser("user-2");

        _quizServiceMock
            .Setup(x => x.GetByIdAsync("quiz-1"))
            .ReturnsAsync(_context.Quizzes.First());

        var result = await _controller.Create(new CreateSessionRequestDto
        {
            QuizId = "quiz-1"
        });

        Assert.IsType<ForbidResult>(result);
    }

    // ACTIVATE WAITING
    [Fact]
    public async Task Activate_ReturnsNoContent_WhenSuccess()
    {
        SetUser("user-1");

        _sessionServiceMock.Setup(x => x.GetByIdAsync("s1"))
            .ReturnsAsync(new Session
            {
                Id = "s1",
                Snapshot = new Snapshot { QuizId = "quiz-1" }
            });

        _quizServiceMock.Setup(x => x.GetByIdAsync("quiz-1"))
            .ReturnsAsync(_context.Quizzes.First());

        _sessionServiceMock.Setup(x => x.ActivateForWaitingByIdAsync("s1"))
            .ReturnsAsync(true);

        var result = await _controller.ActivateForWaitingById("s1");

        Assert.IsType<NoContentResult>(result);
    }

    // GET BY ID
    [Fact]
    public async Task GetById_ReturnsUnauthorized_WhenNoUser()
    {
        SetUser(null);

        var result = await _controller.GetById("s1");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        SetUser("user-1");

        _sessionServiceMock.Setup(x => x.GetByIdAsync("s1"))
            .ReturnsAsync((Session)null!);

        var result = await _controller.GetById("s1");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    // JOIN SESSION 
    [Fact]
    public async Task Join_ReturnsOk_WhenExternalUser()
    {
        SetUser(null);

        var session = new Session
        {
            Id = "s1",
            Status = Sparq.DataAccess.Models.SessionStatus.Waiting,
            Snapshot = new Snapshot
            {
                PinCode = "1234",
                Quiz = new Quiz
                {
                    Id = "q1",
                    IsActive = true,
                    OwnerId = "owner"
                }
            }
        };

        _sessionServiceMock.Setup(x => x.GetByIdAsync("s1"))
            .ReturnsAsync(session);

        _participantServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<Participant>()))
            .ReturnsAsync(new Participant());

        var result = await _controller.JoinSession(new JoinSessionRequestDto
        {
            SessionId = "s1",
            PinCode = "1234",
            Nickname = "Guest"
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}