using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.SnapshotDto;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class SnapshotControllerTests : IDisposable
{
    private readonly SparqDbContext _context;
    private readonly SnapshotController _controller;

    private readonly Mock<ISnapshotService> _snapshotServiceMock = new();
    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<IQuizService> _quizServiceMock = new();

    public SnapshotControllerTests()
    {
        var options = new DbContextOptionsBuilder<SparqDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SparqDbContext(options);

        _controller = new SnapshotController(
            mapper: null!,
            snapshotService: _snapshotServiceMock.Object,
            usersService: _usersServiceMock.Object,
            quizService: _quizServiceMock.Object
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

    // GET BY ID

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _snapshotServiceMock
            .Setup(x => x.GetByIdAsync("bad"))
            .ReturnsAsync((Snapshot?)null);

        var result = await _controller.GetById("bad");

        Assert.IsType<NotFoundResult>(result);
    }

    // CREATE

    [Fact]
    public async Task Create_ReturnsUnauthorized_WhenNoUser()
    {
        SetUser(null);

        var dto = CreateDto();

        var result = await _controller.Create(dto);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsNotFound_WhenQuizMissing()
    {
        SetUser("user-1");

        _quizServiceMock
            .Setup(x => x.GetByIdAsync("quiz-1"))
            .ReturnsAsync((Quiz?)null);

        var dto = CreateDto();

        var result = await _controller.Create(dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsForbid_WhenNotOwner()
    {
        SetUser("user-2");

        _quizServiceMock
            .Setup(x => x.GetByIdAsync("quiz-1"))
            .ReturnsAsync(new Quiz
            {
                Id = "quiz-1",
                OwnerId = "user-1"
            });

        var dto = CreateDto();

        var result = await _controller.Create(dto);

        Assert.IsType<ForbidResult>(result);
    }

    private static SnapshotCreateRequestDto CreateDto()
    {
        return new SnapshotCreateRequestDto
        {
            QuizId = "quiz-1",
            Title = "Snapshot",
            Description = "Desc",
            TimeLimit = 60,
            PinCode = "1234"
        };
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}