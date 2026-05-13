using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.QuestionDto;
using Sparq.Shared.Models.SessionDto;
using Sparq.Shared.Models.SessionQuestion;
using Sparq.SignalR.Services;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Question</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly ISessionService _sessionService;
        private readonly IQuestionService _questionService;
        private readonly IParticipantService _participantService;
        private readonly ISessionQuestionStateService _sessionQuestionStateService;
        private readonly ISessionsNotificationService _sessionsNotificationService;

        /// <summary>Ctor</summary>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="usersService">User service dependency</param>
        /// <param name="questionService">Question service dependency</param>
        /// <param name="participantService">Participant service dependency</param>
        /// <param name="sessionQuestionStateService">Session question state service dependency</param>
        /// <param name="sessionsNotificationService">Session notifier dependency</param>

        public QuestionController(IMapper mapper, IQuestionService questionService,
            ISessionService sessionService, IUsersService usersService, IParticipantService participantService,
            ISessionQuestionStateService sessionQuestionStateService, ISessionsNotificationService sessionsNotificationService)
        {
            _mapper = mapper;
            _questionService = questionService;
            _sessionService = sessionService;
            _usersService = usersService;
            _participantService = participantService;
            _sessionQuestionStateService = sessionQuestionStateService;
            _sessionsNotificationService = sessionsNotificationService;
        }

        [HttpGet("{sessionId}/without-result")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentSessionQuestionStateWithoutResultDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentQuestionWithoutResult(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            var isOwner = user != null &&
                          user.Id == session.Snapshot!.Quiz!.OwnerId;

            var isParticipant = false;

            if (!isOwner)
            {
                if (user == null && !string.IsNullOrWhiteSpace(extUserId))
                {
                    var participant =
                        await _participantService
                            .GetIdByExtUserIdAndSessionIdAsync(extUserId, sessionId);

                    isParticipant = participant != null;
                }
                else if (user != null)
                {
                    var participant =
                        await _participantService
                            .GetIdByUserIdAndSessionIdAsync(user.Id, sessionId);

                    isParticipant = participant != null;
                }

                if (!isParticipant)
                    return Forbid();
            }

            var questionState =
                await _sessionQuestionStateService
                    .GetActiveBySessionIdAsync(sessionId);

            if (questionState == null)
                return NotFound();

            var result =
                _mapper.Map<CurrentSessionQuestionStateWithoutResultDto>(questionState);

            return Ok(result);
        }

        [HttpGet("{sessionId}/with-result")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentSessionQuestionStateWithResultDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentQuestionWithResult(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            var questionState =
                await _sessionQuestionStateService
                    .GetActiveBySessionIdAsync(sessionId);

            if (questionState == null)
                return NotFound();

            var isOwner = user != null &&
                          user.Id == session.Snapshot!.Quiz!.OwnerId;

            var isParticipant = false;

            if (!isOwner)
            {
                if (user == null && !string.IsNullOrWhiteSpace(extUserId))
                {
                    var participant =
                        await _participantService
                            .GetIdByExtUserIdAndSessionIdAsync(extUserId, sessionId);

                    isParticipant = participant != null;
                }
                else if (user != null)
                {
                    var participant =
                        await _participantService
                            .GetIdByUserIdAndSessionIdAsync(user.Id, sessionId);

                    isParticipant = participant != null;
                }

                if (!isParticipant)
                    return Forbid();

                // 🔥 EZ A LÉNYEG:
                // csak akkor láthatja a resultot, ha lejárt az idő
                if (questionState.EndsAt != null &&
                    questionState.EndsAt > DateTime.UtcNow)
                {
                    return Forbid();
                }
            }

            var result =
                _mapper.Map<CurrentSessionQuestionStateWithResultDto>(questionState);

            return Ok(result);
        }
    }
}
