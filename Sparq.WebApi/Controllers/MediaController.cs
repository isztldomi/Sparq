using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.MediaDto;
using System.Security.Claims;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Media</summary>
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IUsersService _usersService;

        /// <summary>Ctor</summary>
        /// <param name="mediaService">Media service dependency</param>
        /// <param name="usersService">User service dependency</param>
        public MediaController(IMediaService mediaService, IUsersService usersService)
        {
            _mediaService = mediaService;
            _usersService = usersService;
        }

        /// <summary>Upload file</summary>
        /// <param name="file">Uploaded file</param>
        /// <returns>Uploaded media metadata</returns>
        /// <remarks>Uploads a file for the authenticated user.</remarks>
        [HttpPost("upload")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MediaUploadResponseDto>> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Empty file");

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Forbid();

            var result = await _mediaService.UploadAsync(file, user.Id);

            var dto = new MediaUploadResponseDto
            {
                Id = result.Id,
                FileName = result.FileName,
                ContentType = result.ContentType,
            };

            return Ok(dto);
        }

        /// <summary>Get file</summary>
        /// <param name="id">Media identifier</param>
        /// <returns>File stream</returns>
        /// <remarks>Returns a file by id for the authenticated user.</remarks>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(MediaFileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Forbid();

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
