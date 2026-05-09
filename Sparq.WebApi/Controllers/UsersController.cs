using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models;
using Sparq.Shared.Models.LoginDto;
using Sparq.Shared.Models.TokenDto;
using Sparq.Shared.Models.UserDto;
using System.ComponentModel.DataAnnotations;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Users</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        
        /// <summary>Ctor</summary>
        /// <param name="mapper">Mapper for DTO conversion</param>
        /// <param name="usersService">User service dependency</param>
        public UsersController(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        /// <summary>Current user</summary>
        /// <returns>Currently authenticated user.</returns>
        /// <remarks>Returns the user identified by the access token.</remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentUser()
        {

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserResponseDto>(user);

            return Ok(dto);
        }

        /// <summary>Get user</summary>
        /// <param name="id">User identifier</param>
        /// <returns>User data.</returns>
        /// <remarks>Returns a user by their unique identifier.</remarks>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _usersService.GetUserByIdAsync(id);
            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return Ok(userResponseDto);
        }

        /// <summary>Create user</summary>
        /// <param name="userRequestDto">User creation payload</param>
        /// <returns>The created user.</returns>
        /// <remarks>Registers a new user in the system.</remarks>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            var user = _mapper.Map<User>(userRequestDto);

            var result = await _usersService.AddUserAsync(user, userRequestDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
            { "User", result.Errors.Select(e => e.Description).ToArray() }
                    }
                ));
            }

            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return CreatedAtAction(nameof(CreateUser), userResponseDto);
        }

        /// <summary>Login</summary>
        /// <param name="loginRequestDto">Login credentials</param>
        /// <returns>Authentication tokens.</returns>
        /// <remarks>Authenticates user and returns JWT + refresh token.</remarks>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var (token, refreshToken, userId, error) =
                await _usersService.LoginAsync(loginRequestDto.Email, loginRequestDto.Password);

            if (error != null)
            {
                return BadRequest(new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                { "User", new[] { "Invalid email or password" } }
                    })
                {
                    Title = "Login failed",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(new LoginResponseDto
            {
                UserId = userId!,
                AuthToken = token!,
                RefreshToken = refreshToken!
            });
        }

        /// <summary>Refresh token</summary>
        /// <param name="redeemRefreshTokenRequestDto">Refresh token request</param>
        /// <returns>New authentication tokens.</returns>
        /// <remarks>Generates new JWT and refresh token pair.</remarks>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RedeemRefreshToken(
            [FromBody] RedeemRefreshTokenRequestDto redeemRefreshTokenRequestDto)
        {
            var refreshToken = redeemRefreshTokenRequestDto.RefreshToken;

            var (authToken, newRefreshToken, userId, error) =
                await _usersService.RedeemRefreshTokenAsync(refreshToken);

            if (error != null)
            {
                return Unauthorized(new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                { "RefreshToken", new[] { error } }
                    })
                {
                    Title = "Authentication failed",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var loginResponseDto = new LoginResponseDto
            {
                UserId = userId!,
                AuthToken = authToken!,
                RefreshToken = newRefreshToken!,
            };

            return Ok(loginResponseDto);
        }

        /// <summary>Logout</summary>
        /// <returns>No content.</returns>
        /// <remarks>Invalidates the current user session.</remarks>
        [HttpPost]
        [Route("logout")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(void))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();

            return NoContent();
        }

        /// <summary>Update nickname</summary>
        /// <param name="nickNameUpdateRequestDto">Nickname update request</param>
        /// <returns>Updated user.</returns>
        /// <remarks>Updates the nickname of the current authenticated user.</remarks>
        [HttpPatch("nickname")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNickName([FromBody] NickNameUpdateRequestDto nickNameUpdateRequestDto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var updatedUser = await _usersService.UpdateNickNameAsync(user!.Id, nickNameUpdateRequestDto.NickName);

            if (updatedUser == null)
                return NotFound();

            var userResponseDto = _mapper.Map<UserResponseDto>(updatedUser);

            return Ok(userResponseDto);
        }

        /// <summary>Optional current user</summary>
        /// <returns>Currently authenticated user or null.</returns>
        /// <remarks>
        /// Returns the current user if authenticated;
        /// otherwise returns null.
        /// </remarks>
        [HttpGet("current")]
        public async Task<IActionResult> GetOptionalCurrentUser()
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Ok(null);

            var dto = _mapper.Map<UserResponseDto>(user);

            return Ok(dto);
        }
    }
}
