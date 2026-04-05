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
    /// <summary>
    /// Controller responsible for user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance for DTO-entity conversions.</param>
        /// <param name="usersService">Service handling user business logic.</param>
        public UsersController(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        /// <summary>
        /// User by ID
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User data.</returns>
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

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="userRequestDto">User creation request data.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            var user = _mapper.Map<User>(userRequestDto);

            await _usersService.AddUserAsync(user, userRequestDto.Password);

            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return StatusCode(StatusCodes.Status201Created, userResponseDto);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequestDto">Login credentials.</param>
        /// <returns>Authentication and refresh tokens.</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var (authToken, refreshToken, userId) = await _usersService.LoginAsync(loginRequestDto.Email, loginRequestDto.Password);

            var loginResponseDto = new LoginResponseDto
            {
                UserId = userId,
                AuthToken = authToken,
                RefreshToken = refreshToken,
            };

            return Ok(loginResponseDto);
        }

        /// <summary>
        /// Token
        /// </summary>
        /// <param name="redeemRefreshTokenRequestDto"></param>
        /// <returns>New authentication and refresh tokens.</returns>
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RedeemRefreshToken([FromBody] RedeemRefreshTokenRequestDto redeemRefreshTokenRequestDto)
        {
            var refreshToken = redeemRefreshTokenRequestDto.RefreshToken;

            var (authToken, newRefreshToken, userId) = await _usersService.RedeemRefreshTokenAsync(refreshToken);

            var loginResponseDto = new LoginResponseDto
            {
                UserId = userId,
                AuthToken = authToken,
                RefreshToken = newRefreshToken,
            };

            return Ok(loginResponseDto);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>No content.</returns>
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
    }
}
