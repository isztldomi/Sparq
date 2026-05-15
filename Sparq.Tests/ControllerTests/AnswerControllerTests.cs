using Microsoft.AspNetCore.Mvc;
using Moq;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.AnswerDto;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class AnswerControllerTests
{
    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<IParticipantService> _participantServiceMock = new();
    private readonly Mock<ISessionQuestionStateService> _sessionQuestionStateServiceMock = new();
    private readonly Mock<IQuestionService> _questionServiceMock = new();
    private readonly Mock<IAnswerService> _answerServiceMock = new();
    private readonly Mock<IParticipantAnswerService> _participantAnswerServiceMock = new();

    private readonly AnswerController _controller;

    public AnswerControllerTests()
    {
        _controller = new AnswerController(
            _usersServiceMock.Object,
            _participantServiceMock.Object,
            _sessionQuestionStateServiceMock.Object,
            _questionServiceMock.Object,
            _answerServiceMock.Object,
            _participantAnswerServiceMock.Object
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
                    NickName = "Test"
                });
    }

    private static SessionQuestionState CreateActiveState()
    {
        return new SessionQuestionState
        {
            Id = "state-1",
            SessionId = "session-1",
            QuestionId = "question-1",
            EndsAt = DateTime.UtcNow.AddMinutes(1)
        };
    }

    // SUBMIT ANSWER

    [Fact]
    public async Task SubmitAnswer_ReturnsNotFound_WhenStateMissing()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync((SessionQuestionState?)null);

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1"
        });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsForbid_WhenQuestionExpired()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(new SessionQuestionState
            {
                EndsAt = DateTime.UtcNow.AddMinutes(-1)
            });

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1"
        });

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsForbid_WhenExternalWithoutExtUserId()
    {
        SetUser(null);

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1"
        });

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsForbid_WhenParticipantMissing()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync((Participant?)null);

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1"
        });

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsBadRequest_WhenAnswerInvalid()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync(new Participant
            {
                Id = "participant-1"
            });

        _answerServiceMock
            .Setup(x => x.GetByIdAsync("answer-1"))
            .ReturnsAsync((Answer?)null);

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1",
            QuestionId = "question-1",
            AnswerId = "answer-1"
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal("Invalid answer", badRequest.Value);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsConflict_WhenAlreadyAnswered()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync(new Participant
            {
                Id = "participant-1"
            });

        _answerServiceMock
            .Setup(x => x.GetByIdAsync("answer-1"))
            .ReturnsAsync(new Answer
            {
                Id = "answer-1",
                QuestionId = "question-1",
                IsCorrect = true
            });

        _participantAnswerServiceMock
            .Setup(x => x.GetParticipantAnswerAsync(
                "session-1",
                "question-1",
                "user-1",
                null))
            .ReturnsAsync(new ParticipantAnswer());

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1",
            QuestionId = "question-1",
            AnswerId = "answer-1"
        });

        var conflict = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal("Already answered", conflict.Value);
    }

    [Fact]
    public async Task SubmitAnswer_ReturnsOk_WhenValid()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync(new Participant
            {
                Id = "participant-1"
            });

        _answerServiceMock
            .Setup(x => x.GetByIdAsync("answer-1"))
            .ReturnsAsync(new Answer
            {
                Id = "answer-1",
                QuestionId = "question-1",
                IsCorrect = true
            });

        _participantAnswerServiceMock
            .Setup(x => x.GetParticipantAnswerAsync(
                "session-1",
                "question-1",
                "user-1",
                null))
            .ReturnsAsync((ParticipantAnswer?)null);

        _questionServiceMock
            .Setup(x => x.GetByIdAsync("question-1"))
            .ReturnsAsync(new Question
            {
                Id = "question-1",
                Point = 100
            });

        var result = await _controller.SubmitAnswer(new SubmitAnswerRequestDto
        {
            SessionId = "session-1",
            QuestionId = "question-1",
            AnswerId = "answer-1"
        });

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(true, ok.Value);

        _participantAnswerServiceMock.Verify(
            x => x.CreateAsync(It.IsAny<ParticipantAnswer>()),
            Times.Once);
    }

    // GET SESSION QUESTION ANSWERS

    [Fact]
    public async Task GetSessionQuestionAnswers_ReturnsNotFound_WhenStateMissing()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync((SessionQuestionState?)null);

        var result = await _controller.GetSessionQuestionAnswers(
            "session-1",
            "question-1",
            null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSessionQuestionAnswers_ReturnsForbid_WhenParticipantMissing()
    {
        SetUser("user-1");

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync((Participant?)null);

        var result = await _controller.GetSessionQuestionAnswers(
            "session-1",
            "question-1",
            null);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetSessionQuestionAnswers_ReturnsOk_WhenValid()
    {
        SetUser("user-1");

        var participant = new Participant
        {
            Id = "participant-1",
            UserId = "user-1"
        };

        _sessionQuestionStateServiceMock
            .Setup(x => x.GetActiveBySessionIdAsync("session-1"))
            .ReturnsAsync(CreateActiveState());

        _participantServiceMock
            .Setup(x => x.GetIdByUserIdAndSessionIdAsync("user-1", "session-1"))
            .ReturnsAsync(participant);

        _participantAnswerServiceMock
            .Setup(x => x.GetBySessionAndQuestionAsync(
                "session-1",
                "question-1"))
            .ReturnsAsync(new List<ParticipantAnswer>
            {
                new()
                {
                    ParticipantId = "participant-1",
                    Participant = participant,
                    AnswerId = "answer-1",
                    IsCorrect = true,
                    PointsEarned = 100,
                    AnsweredAt = DateTime.UtcNow
                }
            });

        _answerServiceMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<Answer>
            {
                new()
                {
                    Id = "answer-1",
                    Text = "Correct answer"
                }
            });

        var result = await _controller.GetSessionQuestionAnswers(
            "session-1",
            "question-1",
            null);

        var ok = Assert.IsType<OkObjectResult>(result);

        var dto = Assert.IsType<SessionQuestionAnswersResponseDto>(ok.Value);

        Assert.Single(dto.Answers);
    }
}