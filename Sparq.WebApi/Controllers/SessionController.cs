using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Services;
using System.Xml.Linq;

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
        /// <param name="sessionsNotificationService">Session notifier dependency</param>
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

        [HttpGet("{sessionId}/public")]
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

        [HttpPatch("{sessionId}/nextQuestion")]
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

                return Ok(true);
            }
            else
            {
                var currentQuestionState = await _sessionQuestionStateService.GetActiveBySessionIdAsync(sessionId);
                if (currentQuestionState == null)
                    return Ok(false);
                if (currentQuestionState!.EndsAt > DateTime.UtcNow)
                    return BadRequest("Current question is still active.");
                await _sessionQuestionStateService.DeactivateCurrentAsync(sessionId);
                var question = await _questionService
                    .GetBySessionIdAndOrderAsync(sessionId, session.CurrentQuestion!.Order + 1);

                if (question == null)
                    return Ok(false);

                session.CurrentQuestionId = question.Id;

                await _sessionQuestionStateService.CreateAsync(sessionId, question.Id);

                await _sessionService.UpdateAsync(session.Id, session);

                return Ok(true);
            }
        }

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
                        ParticipantId = g.Key,
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
    }
} 
