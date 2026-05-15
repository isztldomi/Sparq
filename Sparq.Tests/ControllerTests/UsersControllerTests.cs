using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sparq.DataAccess;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.LoginDto;
using Sparq.Shared.Models.TokenDto;
using Sparq.Shared.Models.UserDto;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class UsersControllerTests : IDisposable
{
    private readonly SparqDbContext _context;
    private readonly UsersController _controller;
    private readonly Mock<IUsersService> _usersServiceMock = new();

    public UsersControllerTests()
    {
        var options = new DbContextOptionsBuilder<SparqDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SparqDbContext(options);

        _controller = new UsersController(
            mapper: null!,
            usersService: _usersServiceMock.Object
        );

        Seed();
    }

    // MOCK USER
    private void SetCurrentUser(User? user)
    {
        _usersServiceMock
            .Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(user);
    }

    private void SetupUpdateNickname(User? user)
    {
        _usersServiceMock
            .Setup(x => x.UpdateNickNameAsync(It.IsAny<string>(), It.IsAny<string>()))!
            .ReturnsAsync(user);
    }

    // SEED
    private void Seed()
    {
        _context.Users.Add(new User
        {
            Id = "user-1",
            Email = "test@test.com",
            UserName = "test",
            NickName = "oldnick"
        });

        _context.SaveChanges();
    }

    // GET CURRENT USER

    [Fact]
    public async Task GetCurrentUser_ReturnsNotFound_WhenNull()
    {
        SetCurrentUser(null);

        var result = await _controller.GetCurrentUser();

        Assert.IsType<NotFoundResult>(result);
    }

    // LOGIN

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenError()
    {
        _usersServiceMock
            .Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((null, null, null, "error"));

        var dto = new LoginRequestDto
        {
            Email = "a",
            Password = "b"
        };

        var result = await _controller.Login(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenSuccess()
    {
        _usersServiceMock
            .Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(("token", "refresh", "user-1", null));

        var dto = new LoginRequestDto
        {
            Email = "a",
            Password = "b"
        };

        var result = await _controller.Login(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    // REFRESH TOKEN

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenError()
    {
        _usersServiceMock
            .Setup(x => x.RedeemRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((null, null, null, "invalid"));

        var dto = new RedeemRefreshTokenRequestDto
        {
            RefreshToken = "bad"
        };

        var result = await _controller.RedeemRefreshToken(dto);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Refresh_ReturnsOk_WhenSuccess()
    {
        _usersServiceMock
            .Setup(x => x.RedeemRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(("token", "refresh", "user-1", null));

        var dto = new RedeemRefreshTokenRequestDto
        {
            RefreshToken = "ok"
        };

        var result = await _controller.RedeemRefreshToken(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    // LOGOUT

    [Fact]
    public async Task Logout_ReturnsNoContent()
    {
        var result = await _controller.Logout();

        Assert.IsType<NoContentResult>(result);
    }

    // UPDATE NICKNAME

    [Fact]
    public async Task UpdateNickName_ReturnsNotFound_WhenNull()
    {
        SetCurrentUser(_context.Users.First());

        SetupUpdateNickname(null);

        var dto = new NickNameUpdateRequestDto
        {
            NickName = "new"
        };

        var result = await _controller.UpdateNickName(dto);

        Assert.IsType<NotFoundResult>(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}