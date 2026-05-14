using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Participant;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Services;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Participant controller</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IParticipantService _participantService;
        private readonly IUsersService _usersService;
        private readonly ISessionService _sessionService;
        private readonly ISessionsNotificationService _sessionsNotificationService;

        /// <summary>Ctor</summary>
        /// <param name="mapper">Mapper for DTO conversion</param>
        /// <param name="participantService">Participant service dependency</param>
        /// <param name="usersService">User service dependency</param>
        /// <param name="sessionService">Session service dependency</param>
        /// <param name="sessionsNotificationService">Session notification service dependency</param>
        public ParticipantController(
            IMapper mapper,
            IParticipantService participantService,
            IUsersService usersService,
            ISessionService sessionService,
            ISessionsNotificationService sessionsNotificationService)
        {
            _mapper = mapper;
            _participantService = participantService;
            _usersService = usersService;
            _sessionService = sessionService;
            _sessionsNotificationService = sessionsNotificationService;
        }

        /// <summary>Check if user joined session</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <param name="extUserId">External user identifier (optional)</param>
        /// <returns>Join status of participant.</returns>
        /// <remarks>Returns whether the current user or external user has joined the session.</remarks>
        [HttpGet("{sessionId}/is-joined")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ParticipantIsJoinedResponseDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IsJoined(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            bool isJoined = false;

            // internal user
            if (user != null)
            {
                isJoined = await _participantService.IsUserJoinedAsync(user.Id, sessionId);
            }
            // external user
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                isJoined = await _participantService.IsExtUserJoinedAsync(extUserId, sessionId);
            }

            return Ok(new ParticipantIsJoinedResponseDto
            {
                IsJoined = isJoined
            });
        }

        /// <summary>Get session participants</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <param name="extUserId">External user identifier (optional)</param>
        /// <returns>List of participants.</returns>
        /// <remarks>
        /// Returns all participants in a session.
        /// Owners can always access the full list.
        /// </remarks>
        [HttpGet("{sessionId}/participants")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ParticipantPublicListResponseDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetParticipantsBySessionId(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            // owner always allowed
            if (user != null && session.Snapshot!.Quiz!.OwnerId == user.Id)
            {
                var ownerParticipants = await _participantService.GetBySessionIdAsync(sessionId);

                var ownerResult =
                    _mapper.Map<IReadOnlyCollection<ParticipantPublicListResponseDto>>(ownerParticipants);

                return Ok(ownerResult);
            }

            bool isAllowed = false;

            // internal user
            if (user != null)
            {
                isAllowed = await _participantService.IsUserJoinedAsync(user.Id, sessionId);
            }
            // external user
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                isAllowed = await _participantService.IsExtUserJoinedAsync(extUserId, sessionId);
            }

            if (!isAllowed)
                return Forbid();

            var participants = await _participantService.GetBySessionIdAsync(sessionId);

            var result =
                _mapper.Map<IReadOnlyCollection<ParticipantPublicListResponseDto>>(participants);

            return Ok(result);
        }

        /// <summary>Delete participant from session</summary>
        /// <param name="sessionId">Session identifier</param>
        /// <param name="participantId">Participant identifier</param>
        /// <returns>Result of deletion.</returns>
        /// <remarks>
        /// Removes a participant from a session.
        /// Only session owner can perform this action while session is in Waiting state.
        /// </remarks>
        [HttpDelete("{sessionId}/{participantId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParticipantFromSessionById(
            [FromRoute] string sessionId,
            [FromRoute] string participantId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Forbid();

            var session = await _sessionService.GetByIdAsync(sessionId);

            if (session == null)
                return NotFound();

            if (session.Snapshot!.Quiz!.OwnerId != user.Id)
                return Forbid();

            if (session.Status != DataAccess.Models.SessionStatus.Waiting)
                return Forbid();

            var result = await _participantService.DeleteAsync(participantId);

            await _sessionsNotificationService.NotifySessionParticipantsUpdatedAsync(sessionId);

            return Ok(result);
        }
    }
}