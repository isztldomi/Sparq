using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.MediaDto;
using System.Security.Claims;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Sparq.WebApi.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IUsersService _usersService;

        public MediaController(IMediaService mediaService, IUsersService usersService)
        {
            _mediaService = mediaService;
            _usersService = usersService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<ActionResult<MediaUploadResponseDto>> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Empty file");

            var user = await _usersService.GetCurrentUserAsync();

            if (string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result = await _mediaService.UploadAsync(file, user.Id);

            var dto = new MediaUploadResponseDto
            {
                Id = result.Id,
                FileName = result.FileName,
                ContentType = result.ContentType,
            };

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(MediaFileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result = await _mediaService.GetFileAsync(id, user.Id);

            var dto = new MediaFileResponseDto
            {
                Id = result.Media.Id,
                FileName = result.Media.FileName,
                ContentType = result.Media.ContentType
            };

            Response.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("inline")
                {
                    FileNameStar = dto.FileName
                }.ToString();

            return File(result.Stream, dto.ContentType);
        }
    }
}
