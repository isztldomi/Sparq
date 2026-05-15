using Microsoft.AspNetCore.Mvc;
using Moq;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Participant;
using Sparq.SignalR.Services;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class ParticipantControllerTests
{
    private readonly ParticipantController _controller;

    private readonly Mock<IParticipantService> _participantServiceMock = new();
    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<ISessionService> _sessionServiceMock = new();
    private readonly Mock<ISessionsNotificationService> _notificationMock = new();

    public ParticipantControllerTests()
    {
        _controller = new ParticipantController(
            mapper: null!, // mapper nem kell
            participantService: _participantServiceMock.Object,
            usersService: _usersServiceMock.Object,
            sessionService: _sessionServiceMock.Object,
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

    private Session CreateSession(
        string ownerId = "owner-1",
        SessionStatus status = SessionStatus.Waiting)
    {
        return new Session
        {
            Id = "session-1",
            Status = status,
            Snapshot = new Snapshot
            {
                Quiz = new Quiz
                {
                    OwnerId = ownerId
                }
            }
        };
    }

    // IS JOINED

    [Fact]
    public async Task IsJoined_ReturnsTrue_ForInternalUser()
    {
        SetUser("user-1");

        _participantServiceMock
            .Setup(x => x.IsUserJoinedAsync("user-1", "session-1"))
            .ReturnsAsync(true);

        var result =
            await _controller.IsJoined("session-1");

        var ok = Assert.IsType<OkObjectResult>(result);

        var dto =
            Assert.IsType<ParticipantIsJoinedResponseDto>(ok.Value);

        Assert.True(dto.IsJoined);
    }

    [Fact]
    public async Task IsJoined_ReturnsTrue_ForExternalUser()
    {
        SetUser(null);

        _participantServiceMock
            .Setup(x => x.IsExtUserJoinedAsync("ext-1", "session-1"))
            .ReturnsAsync(true);

        var result =
            await _controller.IsJoined("session-1", "ext-1");

        var ok = Assert.IsType<OkObjectResult>(result);

        var dto =
            Assert.IsType<ParticipantIsJoinedResponseDto>(ok.Value);

        Assert.True(dto.IsJoined);
    }

    [Fact]
    public async Task IsJoined_ReturnsFalse_WhenNotJoined()
    {
        SetUser("user-1");

        _participantServiceMock
            .Setup(x => x.IsUserJoinedAsync("user-1", "session-1"))
            .ReturnsAsync(false);

        var result =
            await _controller.IsJoined("session-1");

        var ok = Assert.IsType<OkObjectResult>(result);

        var dto =
            Assert.IsType<ParticipantIsJoinedResponseDto>(ok.Value);

        Assert.False(dto.IsJoined);
    }

    // GET PARTICIPANTS

    [Fact]
    public async Task GetParticipants_ReturnsNotFound_WhenSessionMissing()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync((Session)null!);

        var result =
            await _controller.GetParticipantsBySessionId("session-1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetParticipants_ReturnsForbid_WhenNotJoined()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _participantServiceMock
            .Setup(x => x.IsUserJoinedAsync("user-1", "session-1"))
            .ReturnsAsync(false);

        var result =
            await _controller.GetParticipantsBySessionId("session-1");

        Assert.IsType<ForbidResult>(result);
    }

    // DELETE PARTICIPANT

    [Fact]
    public async Task DeleteParticipant_ReturnsForbid_WhenNoUser()
    {
        SetUser(null);

        var result =
            await _controller.DeleteParticipantFromSessionById(
                "session-1",
                "participant-1");

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteParticipant_ReturnsNotFound_WhenSessionMissing()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync((Session)null!);

        var result =
            await _controller.DeleteParticipantFromSessionById(
                "session-1",
                "participant-1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteParticipant_ReturnsForbid_WhenNotOwner()
    {
        SetUser("user-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession(ownerId: "owner-1"));

        var result =
            await _controller.DeleteParticipantFromSessionById(
                "session-1",
                "participant-1");

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteParticipant_ReturnsForbid_WhenSessionNotWaiting()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession(
                ownerId: "owner-1",
                status: SessionStatus.Running));

        var result =
            await _controller.DeleteParticipantFromSessionById(
                "session-1",
                "participant-1");

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteParticipant_ReturnsOk_WhenSuccess()
    {
        SetUser("owner-1");

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync("session-1"))
            .ReturnsAsync(CreateSession());

        _participantServiceMock
            .Setup(x => x.DeleteAsync("participant-1"))
            .ReturnsAsync(true);

        var result =
            await _controller.DeleteParticipantFromSessionById(
                "session-1",
                "participant-1");

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(true, ok.Value);

        _notificationMock.Verify(
            x => x.NotifySessionParticipantsUpdatedAsync("session-1"),
            Times.Once);
    }
}