using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.MediaDto;
using Sparq.WebApi.Controllers;

namespace Sparq.Tests.ControllerTests;

public class MediaControllerTests
{
    private readonly Mock<IMediaService> _mediaServiceMock = new();
    private readonly Mock<IUsersService> _usersServiceMock = new();
    private readonly Mock<IParticipantService> _participantServiceMock = new();

    private readonly MediaController _controller;

    public MediaControllerTests()
    {
        _controller = new MediaController(
            _mediaServiceMock.Object,
            _usersServiceMock.Object,
            _participantServiceMock.Object
        );
    }

    // HELPERS

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

    private static IFormFile CreateFile()
    {
        var content = "test content";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        return new FormFile(stream, 0, stream.Length, "file", "test.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
    }

    // UPLOAD

    [Fact]
    public async Task Upload_ReturnsBadRequest_WhenFileIsNull()
    {
        var result = await _controller.Upload(null!);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

        Assert.Equal("Empty file", badRequest.Value);
    }

    [Fact]
    public async Task Upload_ReturnsForbid_WhenNoUser()
    {
        SetUser(null);

        var file = CreateFile();

        var result = await _controller.Upload(file);

        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task Upload_ReturnsOk_WhenUploadSuccessful()
    {
        SetUser("user-1");

        var file = CreateFile();

        _mediaServiceMock
            .Setup(x => x.UploadAsync(file, "user-1"))
            .ReturnsAsync(new Media
            {
                Id = "media-1",
                FileName = "test.png",
                ContentType = "image/png"
            });

        var result = await _controller.Upload(file);

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var dto = Assert.IsType<MediaUploadResponseDto>(ok.Value);

        Assert.Equal("media-1", dto.Id);
        Assert.Equal("test.png", dto.FileName);
    }

    // GET

    [Fact]
    public async Task Get_ReturnsForbid_WhenNoUser()
    {
        SetUser(null);

        var result = await _controller.Get("media-1");

        Assert.IsType<ForbidResult>(result);
    }


    // GET FOR SESSION

    [Fact]
    public async Task GetForSession_ReturnsForbid_WhenExternalUserWithoutExtUserId()
    {
        SetUser(null);

        var result = await _controller.GetForSession(
            "media-1",
            "session-1"
        );

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetForSession_ReturnsForbid_WhenParticipantNotFound()
    {
        SetUser(null);

        _participantServiceMock
            .Setup(x => x.GetIdByExtUserIdAndSessionIdAsync(
                "ext-1",
                "session-1"))
            .ReturnsAsync((Participant?)null);

        var result = await _controller.GetForSession(
            "media-1",
            "session-1",
            "ext-1"
        );

        Assert.IsType<ForbidResult>(result);
    }




}