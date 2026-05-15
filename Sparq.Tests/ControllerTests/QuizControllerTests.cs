using Microsoft.EntityFrameworkCore;
using Moq;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SnapshotDto;
using Sparq.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Sparq.Tests.ControllerTests;

public class QuizControllerTests : IDisposable
{
    private readonly SparqDbContext _context;
    private readonly QuizController _controller;
    private readonly Mock<IUsersService> _usersServiceMock = new();

    public QuizControllerTests()
    {
        var options = new DbContextOptionsBuilder<SparqDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SparqDbContext(options);

        var quizService = new QuizService(_context);

        _controller = new QuizController(
            mapper: null!, // ❗ nem kell Mapper
            quizService,
            _usersServiceMock.Object
        );

        Seed();
    }

    // USER MOCK
    private void SetUser(string? userId)
    {
        _usersServiceMock
            .Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(userId == null
                ? null
                : new User { Id = userId });
    }

    // SEED
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
            IsPublic = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        _context.SaveChanges();
    }

    // GET BY ID

    [Fact]
    public async Task GetById_ReturnsUnauthorized_WhenNoUser()
    {
        SetUser(null);

        var result = await _controller.GetById("quiz-1");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        SetUser("user-1");

        var result = await _controller.GetById("invalid");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsForbid_WhenNotOwner()
    {
        SetUser("user-2");

        var result = await _controller.GetById("quiz-1");

        Assert.IsType<ForbidResult>(result);
    }

    // CREATE

    [Fact]
    public async Task Create_ReturnsUnauthorized_WhenNoUser()
    {
        SetUser(null);

        var dto = CreateValidQuizDto();

        var result = await _controller.Create(dto);

        Assert.IsType<UnauthorizedResult>(result);
    }

    // DEACTIVATE

    [Fact]
    public async Task Deactivate_ReturnsNoContent()
    {
        SetUser("user-1");

        var result = await _controller.Deactivate("quiz-1");

        Assert.IsType<NoContentResult>(result);

        var quiz = _context.Quizzes.First();
        Assert.False(quiz.IsActive);
    }

    // TOGGLE

    [Fact]
    public async Task ToggleVisibility_TogglesFlag()
    {
        SetUser("user-1");

        var result = await _controller.ToggleVisibility("quiz-1");

        Assert.IsType<NoContentResult>(result);

        var quiz = _context.Quizzes.First();
        Assert.True(quiz.IsPublic);
    }

    // DTO

    private static QuizCreateRequestDto CreateValidQuizDto()
    {
        return new QuizCreateRequestDto
        {
            IsPublic = true,
            Snapshots = new List<SnapshotCreateFromQuizRequestDto>
            {
                new()
                {
                    Title = "Snapshot 1",
                    Description = "Desc 1",
                    TimeLimit = 60,
                    PinCode = "1234",
                    Questions = new()
                }
            }
        };
    }

    // CLEANUP

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}