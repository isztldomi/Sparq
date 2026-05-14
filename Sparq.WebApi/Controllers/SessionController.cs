using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Services;
using System.Xml.Linq;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Session controller</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IUsersService _usersService;
        private readonly IQuizService _quizService;
        private readonly ISnapshotService _snapshotService;
        private readonly IParticipantService _participantService;
        private readonly IQuestionService _questionService;
        private readonly ISessionQuestionStateService _sessionQuestionStateService;
        private readonly IParticipantAnswerService _participantAnswerService;
        private readonly ISessionsNotificationService _sessionsNotificationService;

        /// <summary>Ctor</summary>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="sessionService">Session service dependency</param>
        /// <param name="usersService">User service dependency</param>
        /// <param name="quizService">Quiz service dependency</param>
        /// <param name="snapshotService">Snapshot service dependency</param>
        /// <param name="participantService">Participant service dependency</param>
        /// <param name="questionService">Question service dependency</param>
        /// <param name="sessionQuestionStateService">Session question state service dependency</param>
        /// <param name="participantAnswerService">Participant answer service dependency</param>
        /// <param name="sessionsNotificationService">Session notification service dependency</param>
        public SessionController(IMapper mapper, ISessionService sessionService, IUsersService usersService, 
            IQuizService quizService, ISnapshotService snapshotService, IParticipantService participantService,
            IQuestionService questionService, ISessionQuestionStateService sessionQuestionStateService,
            IParticipantAnswerService participantAnswerService, ISessionsNotificationService sessionsNotificationService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _usersService = usersService;
            _quizService = quizService;
            _snapshotService = snapshotService;
            _participantService = participantService;
            _questionService = questionService;
            _sessionQuestionStateService = sessionQuestionStateService;
            _participantAnswerService = participantAnswerService;
            _sessionsNotificationService = sessionsNotificationService;
        }


        /// <summary>Create session</summary>
        /// <param name="createSessionRequestDto">Session creation request</param>
        /// <returns>Created session.</returns>
        /// <remarks>Creates a session from the latest quiz snapshot. Only quiz owner allowed.</remarks>
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

            return NoContent();
        }

        /// <summary>Get all public waiting sessions</summary>
        /// <param name="page">Page number (starts from 1)</param>
        /// <param name="pageSize">Page size (1–100)</param>
        /// <returns>Paginated list of public sessions in waiting state.</returns>
        /// <remarks>
        /// Returns all sessions that are publicly available and currently in waiting state.
        /// Results are paginated.
        /// </remarks>
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

        /// <summary>Get session by id</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>Session details.</returns>
        /// <remarks>
        /// Returns session details only if the current user is the quiz owner.
        /// </remarks>
        [HttpGet("{sessionId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionListDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);
            if (session == null)
                return NotFound("Session not found");

            if (session.Snapshot == null)
                return NotFound("Snapshot not found");

            var quiz = await _quizService.GetByIdAsync(session.Snapshot!.QuizId!);
            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var mapped = _mapper.Map<SessionListDto>(session);

            return Ok(mapped);
        }

        /// <summary>Get public session data</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>Public session information.</returns>
        /// <remarks>
        /// Returns public session data if the session is not in Created state.
        /// No authentication required.
        /// </remarks>
        [HttpGet("{sessionId}/public")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionPublicWaitingListDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSessionPublicDataById([FromRoute] string sessionId)
        {
            var session = await _sessionService.GetByIdAsync(sessionId);
            if (session == null)
                return NotFound("Session not found");

            if (session.Snapshot == null)
                return NotFound("Snapshot not found");

            var quiz = await _quizService.GetByIdAsync(session.Snapshot!.QuizId!);
            if (quiz == null)
                return NotFound("Quiz not found");
            
            // if (quiz.IsPublic == false)
            //     return Forbid();

            if (session.Status == DataAccess.Models.SessionStatus.Created)
                return Forbid();

            var mapped = _mapper.Map<SessionPublicWaitingListDto>(session);

            return Ok(mapped);
        }

        /// <summary>Join session</summary>
        /// <param name="joinSessionRequestDto">Join session request containing session id, pin code and optional nickname.</param>
        /// <returns>External user identifier if user joined as guest.</returns>
        /// <remarks>
        /// Allows a user (authenticated or external) to join a waiting session.
        /// External users must provide a nickname and receive an external user id.
        /// </remarks>
        [HttpPost("join")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JoinSessionExtUserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> JoinSession([FromBody] JoinSessionRequestDto joinSessionRequestDto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(joinSessionRequestDto.SessionId);
            if (session == null)
                return NotFound("Session not found");

            if (session.Snapshot == null)
                return NotFound("Snapshot not found");

            if (session.Snapshot.Quiz == null)
                return NotFound("Quiz not found");

            if (!session.Snapshot.Quiz.IsActive || session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            if (session.Snapshot.Quiz.OwnerId == user?.Id)
                return Forbid();

            if (session.Snapshot.PinCode != joinSessionRequestDto.PinCode)
                return BadRequest("Invalid pin code");

            bool isExternal = user == null;

            var participant = new Participant
            {
                Id = Guid.NewGuid().ToString(),
                UserId = isExternal ? null : user!.Id,
                ExternalUserId = isExternal ? Guid.NewGuid().ToString() : null,
                DisplayName = isExternal
                    ? joinSessionRequestDto.Nickname!.Trim()
                    : user!.NickName,
                SessionId = session.Id,
                Score = 0,
                Rank = 0,
                IsFinished = false,
            };

            await _participantService.CreateAsync(participant);

            await _sessionsNotificationService.NotifySessionParticipantsUpdatedAsync(session.Id);

            return Ok(new JoinSessionExtUserResponseDto
            {
                ExternalUserId = participant.ExternalUserId
            });
        }

        /// <summary>Quit session</summary>
        /// <param name="quitSessionRequestDto">Quit request containing session id and optional external user id.</param>
        /// <returns>True if participant was successfully removed.</returns>
        /// <remarks>
        /// Removes a participant from a session.
        /// Works for both authenticated and external users.
        /// </remarks>
        [HttpPost("quit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> QuitSession([FromBody] QuitSessionRequestDto quitSessionRequestDto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(quitSessionRequestDto.SessionId);
            if (session == null)
                return NotFound("Session not found");

            if (session.Snapshot == null)
                return NotFound("Snapshot not found");

            if (session.Snapshot.Quiz == null)
                return NotFound("Quiz not found");

            if (!session.Snapshot.Quiz.IsActive || session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            if (session.Snapshot.Quiz.OwnerId == user?.Id)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            bool isExternal = user == null;

            Participant? participant = null;

            if (isExternal)
            {
                if (string.IsNullOrWhiteSpace(quitSessionRequestDto.ExternalUserId))
                    return BadRequest("ExternalUserId is required for external users");
                participant = await _participantService.GetIdByExtUserIdAndSessionIdAsync(quitSessionRequestDto.ExternalUserId, quitSessionRequestDto.SessionId);
            }
            else
            {
                participant = await _participantService.GetIdByUserIdAndSessionIdAsync(user!.Id, quitSessionRequestDto.SessionId);
            }

            if (participant == null)
                return NotFound("Participant not found");

            var result = await _participantService.DeleteAsync(participant.Id);

            await _sessionsNotificationService.NotifySessionParticipantsUpdatedAsync(session.Id);

            return Ok(result);
        }

        /// <summary>Get session status</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <param name="extUserId">Optional external user identifier</param>
        /// <returns>Current session status.</returns>
        /// <remarks>
        /// Returns session status for participants or session owner.
        /// Access is restricted to participants or the owner.
        /// </remarks>
        [HttpGet("{sessionId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionStatusResponseDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSessionStatusById(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            // owner


            bool isAllowed = false;

            if (user != null)
            {
                isAllowed = await _participantService.IsUserJoinedAsync(user.Id, sessionId);
            }
            // external user
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                isAllowed = await _participantService.IsExtUserJoinedAsync(extUserId, sessionId);
            }

            if (!isAllowed && (user == null || user.Id != session.Snapshot!.Quiz!.OwnerId))
                return Forbid();

            return Ok(new SessionStatusResponseDto
            {
                Status = (Shared.Models.SessionDto.SessionStatus)session.Status
            });
        }

        /// <summary>Delete session</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>True if deletion succeeded.</returns>
        /// <remarks>
        /// Deletes a session only if it is in Created state and owned by the current user.
        /// </remarks>
        [HttpDelete("{sessionId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSession([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            if (user!.Id != session.Snapshot!.Quiz!.OwnerId)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Created)
                return Forbid();

            var result = await _sessionService.DeleteAsync(sessionId);

            return Ok(result);
        }

        /// <summary>Deactivate session</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>True if deactivation succeeded.</returns>
        /// <remarks>
        /// Removes all participants and deactivates the session.
        /// Only allowed for the quiz owner when session is in Waiting state.
        /// </remarks>
        [HttpPatch("{sessionId}/deactivate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateSession([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            if (user!.Id != session.Snapshot!.Quiz!.OwnerId)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            var participants = await _participantService.GetAllParticipantsBySessionIdAsync(sessionId);

            foreach (var participant in participants)
            {
                await _participantService.DeleteAsync(participant.Id);
            }

            await _sessionsNotificationService.NotifySessionDeactivatedAsync(sessionId);

            var result = await _sessionService.DeactivateSessionAsync(sessionId); 
            
            return Ok(result);
        }

        /// <summary>Start session</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>True if session successfully started.</returns>
        /// <remarks>
        /// Starts a session if it is in Waiting state and has at least one participant.
        /// Only the quiz owner can start the session.
        /// </remarks>
        [HttpPatch("{sessionId}/start")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartSession([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            if (user!.Id != session.Snapshot!.Quiz!.OwnerId)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            var participants = await _participantService.GetAllParticipantsBySessionIdAsync(sessionId);

            if (!participants.Any())
                return Forbid();

            var result = await _sessionService.StartSessionAsync(sessionId);
            await _sessionsNotificationService.NotifySessionStart(sessionId);

            return Ok(result);
        }

        /// <summary>Move to next question</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>True if next question was loaded, false if session ended or no next question exists.</returns>
        /// <remarks>
        /// Advances the session to the next question.
        /// Ends the session if no more questions exist.
        /// </remarks>
        [HttpPatch("{sessionId}/nextQuestion")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> NextQuestionSession([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            if (session.Snapshot!.Quiz!.OwnerId != user.Id)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Running)
                return Forbid();

            if (session.CurrentQuestionId == null)
            {
                var question = await _questionService
                    .GetBySessionIdAndOrderAsync(sessionId, 0);

                if (question == null)
                    return NotFound("First question not found.");

                session.CurrentQuestionId = question.Id;

                await _sessionQuestionStateService.CreateAsync(sessionId, question.Id);

                await _sessionService.UpdateAsync(session.Id, session);

                await _sessionsNotificationService.NotifySessionNextQuestion(sessionId);

                return Ok(true);
            }
            else
            {
                var currentQuestionState = await _sessionQuestionStateService.GetActiveBySessionIdAsync(sessionId);
                if (currentQuestionState == null)
                {
                    return Ok(false);
                }
                if (currentQuestionState!.EndsAt > DateTime.UtcNow)
                    return BadRequest("Current question is still active.");
                await _sessionQuestionStateService.DeactivateCurrentAsync(sessionId);
                var question = await _questionService
                    .GetBySessionIdAndOrderAsync(sessionId, session.CurrentQuestion!.Order + 1);

                if (question == null)
                {
                    await _sessionService.EndSessionAsync(sessionId);
                    await _sessionsNotificationService.NotifySessionEnd(sessionId);
                    await _sessionsNotificationService.NotifySessionNextQuestion(sessionId);
                    return Ok(false);
                }

                session.CurrentQuestionId = question.Id;

                await _sessionQuestionStateService.CreateAsync(sessionId, question.Id);

                await _sessionService.UpdateAsync(session.Id, session);

                await _sessionsNotificationService.NotifySessionNextQuestion(sessionId);

                return Ok(true);
            }
        }

        /// <summary>Get session leaderboard</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <param name="extUserId">Optional external user id</param>
        /// <returns>Leaderboard with participant rankings and scores.</returns>
        /// <remarks>
        /// Returns aggregated results for all participants in the session.
        /// Access is allowed for participants or session owner.
        /// </remarks>
        [HttpGet("{sessionId}/leaderboard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionLeaderboardDto))]
        public async Task<IActionResult> GetSessionLeaderboard(string sessionId, string? extUserId)
        {
            var state = await _sessionQuestionStateService
                .GetActiveBySessionIdAsync(sessionId);

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (state == null || session == null)
                return NotFound();

            var user = await _usersService.GetCurrentUserAsync();

            Participant? participant = null;

            if (user != null)
            {
                participant = await _participantService
                    .GetIdByUserIdAndSessionIdAsync(user.Id, sessionId);
            }
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                participant = await _participantService
                    .GetIdByExtUserIdAndSessionIdAsync(extUserId, sessionId);
            }

            var isOwner = user != null && session.Snapshot!.Quiz!.OwnerId == user.Id;

            var isParticipant = participant != null;

            if (!isOwner && !isParticipant)
                return Forbid();

            var allParticipantAnswers =
                await _participantAnswerService
                    .GetBySessionAsync(sessionId);

            var grouped = allParticipantAnswers
                .GroupBy(x => x.ParticipantId)
                .Select(g =>
                {
                    var first = g.First();

                    return new LeaderboardEntryDto
                    {
                        ParticipantId = g.Key!,
                        DisplayName = first.Participant!.DisplayName,
                        UserId = first.Participant?.UserId,
                        ExtUserId = first.Participant?.ExternalUserId,

                        TotalPoints = g.Sum(x => x.PointsEarned),
                        CorrectAnswers = g.Count(x => x.IsCorrect)
                    };
                })
                .OrderByDescending(x => x.TotalPoints)
                .ThenByDescending(x => x.CorrectAnswers)
                .ToList();

            return Ok(new SessionLeaderboardDto
            {
                SessionId = sessionId,
                Entries = grouped
            });
        }

        /// <summary>Get session history</summary>
        /// <param name="page">Page number (starts from 1)</param>
        /// <param name="pageSize">Page size (1–100)</param>
        /// <returns>Paginated list of sessions participated by the current user.</returns>
        /// <remarks>
        /// Returns historical sessions where the current user participated.
        /// </remarks>
        [HttpGet("history")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<MySessionListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid paging parameters.");

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var baseQuery = _participantService.GetUserSessionQuery(user.Id);

            var totalCount = await baseQuery
                .Select(p => p.SessionId)
                .Where(id => id != null)
                .Distinct()
                .CountAsync();

            var items = await baseQuery
                .Where(p => p.Session != null && p.SessionId != null)
                .Select(p => new MySessionListDto
                {
                    SnapshotTitle = p.Session!.Snapshot!.Title!,
                    SessionId = p.SessionId!,
                    StartedAt = p.Session.StartedAt != null ? (DateTime)p.Session.StartedAt : DateTime.MaxValue, // ha még nem kezdődött volna el
                })
                .Distinct()
                .OrderByDescending(x => x.StartedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<MySessionListDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
    }
} 
