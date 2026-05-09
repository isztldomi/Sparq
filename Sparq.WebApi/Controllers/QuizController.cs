using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SessionDto;
using System.Security.Claims;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Quiz API</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuizService _quizService;
        private readonly IUsersService _usersService;

        /// <summary>Ctor</summary>
        /// <param name="mapper">Mapper for DTO mapping</param>
        /// <param name="quizService">Quiz service dependency</param>
        /// <param name="usersService">User service dependency</param>
        public QuizController(IMapper mapper, IQuizService quizService, IUsersService usersService)
        {
            _mapper = mapper;
            _quizService = quizService;
            _usersService = usersService;
        }

        /// <summary>Get quiz</summary>
        /// <param name="id">Quiz identifier</param>
        /// <returns>The requested quiz.</returns>
        /// <remarks>Returns the quiz only if the current user is the owner.</remarks>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var quizEntity = await _quizService.GetByIdAsync(id);

            if (quizEntity == null)
                return NotFound();

            if (quizEntity.OwnerId != user.Id)
            {
                return Forbid();
            }

            var response = _mapper.Map<QuizResponseDto>(quizEntity);

            return Ok(response);
        }

        /// <summary>Get all quizzes</summary>
        /// <returns>List of quizzes.</returns>
        /// <remarks>Returns all quizzes in the system.</remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<QuizResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var quizEntities = await _quizService.GetAllAsync();
            var response = _mapper.Map<List<QuizResponseDto>>(quizEntities);
            return Ok(response);
        }

        /// <summary>Create quiz</summary>
        /// <param name="quizCreateRequestDto">Quiz creation payload</param>
        /// <returns>The created quiz.</returns>
        /// <remarks>
        /// Creates a quiz with snapshots and assigns the current user as owner.
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuizResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] QuizCreateRequestDto quizCreateRequestDto)
        {
            var quiz = _mapper.Map<Quiz>(quizCreateRequestDto);

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            //foreach (var snapshot in quiz.Snapshots!)
            //{
            //    snapshot.CreatedAt = DateTime.UtcNow;
            //}

            quiz.OwnerId = user.Id;

            var savedQuiz = await _quizService.CreateAsync(quiz);

            var quizResponseDto = _mapper.Map<QuizResponseDto>(savedQuiz);

            return CreatedAtAction(nameof(GetById), new { id = quizResponseDto.Id }, quizResponseDto);
        }

        /// <summary>My quizzes</summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paginated quizzes.</returns>
        /// <remarks>Returns quizzes owned by the current user.</remarks>
        [HttpGet("mine")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<MyQuizListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyQuizzes(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid paging parameters.");

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var (items, totalCount) = await _quizService.GetByUserPagedAsync(user.Id, page, pageSize);

            var mapped = _mapper.Map<List<MyQuizListDto>>(items);

            var result = new PagedResult<MyQuizListDto>
            {
                Items = mapped,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        /// <summary>Deactivate quiz</summary>
        /// <param name="id">Quiz identifier</param>
        /// <returns>No content.</returns>
        /// <remarks>Deactivates the quiz owned by the current user.</remarks>
        [HttpPatch("{id}/deactivate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate(string id)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var quiz = await _quizService.GetByIdAsync(id);

            if (quiz == null)
                return NotFound();

            if (quiz.OwnerId != user.Id)
                return Forbid();

            await _quizService.DeactivateAsync(id, user.Id);

            return NoContent();
        }

        /// <summary>Quiz sessions</summary>
        /// <param name="quizId">Quiz identifier</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paginated sessions.</returns>
        /// <remarks>Returns sessions belonging to a quiz.</remarks>
        [HttpGet("{quizId}/sessions")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<SessionListDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSessions(
            string quizId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var quiz = await _quizService.GetByIdAsync(quizId);

            if (quiz == null || !quiz.IsActive)
                return NotFound();

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var (items, totalCount) = await _quizService
                .GetQuizSessionsPagedAsync(quizId, page, pageSize);

            var mapped = _mapper.Map<List<SessionListDto>>(items);

            var result = new PagedResult<SessionListDto>
            {
                Items = mapped,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpPatch("{quizId}/toggle-visibility")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleVisibility(string quizId)
        {
            var user = await _usersService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();
            var quiz = await _quizService.GetByIdAsync(quizId);
            if (quiz == null)
                return NotFound();
            if (quiz.OwnerId != user.Id)
                return Forbid();
            quiz.IsPublic = !quiz.IsPublic;
            await _quizService.UpdateAsync(quizId, quiz);
            return NoContent();
        }
    }
}