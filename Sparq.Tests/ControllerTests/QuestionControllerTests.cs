using Microsoft.AspNetCore.Mvc;
using Moq;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.QuestionDto;
using Sparq.Shared.Models.SessionQuestion;
using Sparq.SignalR.Services;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class QuestionControllerTests
{
    private readonly QuestionController _controller;

    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<ISessionService> _sessionServiceMock = new();
    private readonly Mock<IQuestionService> _questionServiceMock = new();
    private readonly Mock<IParticipantService> _participantServiceMock = new();
    private readonly Mock<ISessionQuestionStateService> _stateServiceMock = new();
    private readonly Mock<ISessionsNotificationService> _notificationMock = new();

    public QuestionControllerTests()
    {
        _controller = new QuestionController(
            mapper: null!, // mapper nem kell
            questionService: _questionServiceMock.Object,
            sessionService: _sessionServiceMock.Object,
            usersService: _usersServiceMock.Object,
            participantService: _participantServiceMock.Object,
            sessionQuestionStateService: _stateServiceMock.Object,
            sessionsNotificationService: _notificationMock.Object
        );
    }

    private void SetUser(string? userId)
    {
        _usersServiceMock
            .Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(userId == null
                ? null
                : new User
                {
                    Id = userId,
                    NickName = "TestUser"
                });
    }

    private Session CreateSession(string ownerId = "owner-1")
    {
        return new Session
        {
            Id = "session-1",
            Snapshot = new Snapshot
            {
                Quiz = new Quiz
                {
                    OwnerId = ownerId
                }
            }
        };
    }

    private SessionQuestionState CreateQuestionState(bool ended = false)
    {
        return new SessionQuestionState
        {
            Id = "state-1",
            SessionId = "session-1",
            QuestionId = "question-1",
            EndsAt = ended
                ? DateTime.UtcNow.AddMinutes(-1)
                : DateTime.UtcNow.AddMinutes(5)
        };
    }

    // GET WITHOUT RESULT

    [Fact]
    public async Task GetWithoutResult_ReturnsNotFound_WhenSessionMissing()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync((Session)null!);

        var result =
            await _controller.GetCurrentQuestionWithoutResult("session-1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetWithoutResult_ReturnsForbid_WhenNotOwnerOrParticipant()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession("owner-1"));

        _participantServiceMock
            .Setup(x =>
                x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync((Participant)null!);

        var result =
            await _controller.GetCurrentQuestionWithoutResult("session-1");

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetWithoutResult_ReturnsNotFound_WhenNoQuestionState()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _stateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync((SessionQuestionState)null!);

        var result =
            await _controller.GetCurrentQuestionWithoutResult("session-1");

        Assert.IsType<NotFoundResult>(result);
    }

    // GET WITH RESULT

    [Fact]
    public async Task GetWithResult_ReturnsNotFound_WhenSessionMissing()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync((Session)null!);

        var result =
            await _controller.GetCurrentQuestionWithResult("session-1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetWithResult_ReturnsNotFound_WhenQuestionStateMissing()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _stateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync((SessionQuestionState)null!);

        var result =
            await _controller.GetCurrentQuestionWithResult("session-1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetWithResult_ReturnsForbid_WhenNotParticipant()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _stateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateQuestionState(true));

        _participantServiceMock
            .Setup(x =>
                x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync((Participant)null!);

        var result =
            await _controller.GetCurrentQuestionWithResult("session-1");

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetWithResult_ReturnsForbid_WhenQuestionStillActive()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _participantServiceMock
            .Setup(x =>
                x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync(new Participant
            {
                Id = "participant-1"
            });

        _stateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateQuestionState(false));

        var result =
            await _controller.GetCurrentQuestionWithResult("session-1");

        Assert.IsType<ForbidResult>(result);
    }
}