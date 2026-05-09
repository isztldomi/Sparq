using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Notifiers;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Session</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IUsersService _usersService;
        private readonly IQuizService _quizService;
        private readonly ISnapshotService _snapshotService;
        private readonly ISessionNotifier _sessionNotifier;

        /// <summary>Ctor</summary>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="sessionService">Session service dependency</param>
        /// <param name="usersService">User service dependency</param>
        /// <param name="quizService">Quiz service dependency</param>
        /// <param name="snapshotService">Snapshot service dependency</param>
        /// <param name="sessionNotifier">Session notifier dependency</param>
        public SessionController(IMapper mapper, ISessionService sessionService, IUsersService usersService, IQuizService quizService, ISnapshotService snapshotService, ISessionNotifier sessionNotifier)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _usersService = usersService;
            _quizService = quizService;
            _snapshotService = snapshotService;
            _sessionNotifier = sessionNotifier;
        }

        /// <summary>Create session</summary>
        /// <param name="createSessionRequestDto">Session creation request containing quiz identifier.</param>
        /// <returns>The created session.</returns>
        /// <remarks>
        /// Creates a new session from the latest snapshot of the specified quiz.
        /// Only the quiz owner is allowed to create sessions.
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateSessionResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateSessionRequestDto createSessionRequestDto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var quiz = await _quizService.GetByIdAsync(createSessionRequestDto.QuizId);

            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var snapshot = await _snapshotService.GetByIdAsync(quiz.LastSnapshot!.Id);

            if (snapshot == null)
                return NotFound("Snapshot not found");

            var session = await _sessionService.CreateAsync(snapshot.Id);

            if (session == null)
                return NotFound("Snapshot not found");

            var result = _mapper.Map<CreateSessionResponseDto>(session);

            return Ok(result);
        }

        /// <summary>Activate session waiting state</summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <remarks>
        /// Sets the session into waiting state (IsWaiting = true).
        /// Only the quiz owner is allowed to activate the session.
        /// Returns 204 No Content if the operation succeeds.
        /// </remarks>
        [HttpPatch("{sessionId}/activate-waiting")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateForWaitingById([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);
            if (session == null)
                return NotFound("Session not found");

            var quiz = await _quizService.GetByIdAsync(session.Snapshot!.QuizId!);
            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var success = await _sessionService.ActivateForWaitingByIdAsync(sessionId);

            if (!success)
                return NotFound("Session not found");

            await _sessionNotifier.SessionWaitingActivated(sessionId);

            return NoContent();
        }

        [HttpGet("public-waiting")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<SessionPublicWaitingListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPublicWaitingSessions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);


            var (items, totalCount) = await _sessionService
                .GetAllPublicWaitingSessionsPagedAsync( page, pageSize);


            var mapped = _mapper.Map<List<SessionPublicWaitingListDto>>(items);

            var result = new PagedResult<SessionPublicWaitingListDto>
            {
                Items = mapped,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }
    }
} 
