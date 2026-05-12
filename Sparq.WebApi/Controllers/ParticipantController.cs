using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Participant;
using Sparq.Shared.Models.SessionDto;
using Sparq.SignalR.Services;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Media</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IParticipantService _participantService;
        private readonly IUsersService _usersService;
        private readonly ISessionService _sessionService;
        private readonly ISessionsNotificationService _sessionsNotificationService;

        public ParticipantController(IMapper mapper, IParticipantService participantService, IUsersService usersService, ISessionService sessionService, ISessionsNotificationService sessionsNotificationService)
        {
            _mapper = mapper;
            _participantService = participantService;
            _usersService = usersService;
            _sessionService = sessionService;
            _sessionsNotificationService = sessionsNotificationService;
        }

        [HttpGet("{sessionId}/is-joined")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ParticipantIsJoinedResponseDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IsJoined(
            [FromRoute] string sessionId,
            [FromQuery] string? extUserId = null)
        {
            var user = await _usersService.GetCurrentUserAsync();

            bool isJoined = false;

            // belső user
            if (user != null)
            {
                isJoined = await _participantService.IsUserJoinedAsync(user.Id, sessionId);
            }
            // external user
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                isJoined = await _participantService.IsExtUserJoinedAsync(extUserId, sessionId);
            }

            var result = new ParticipantIsJoinedResponseDto
            {
                IsJoined = isJoined
            };

            return Ok(result);
        }

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

            // owner mindig láthatja
            if (user != null && session.Snapshot.Quiz.OwnerId == user.Id)
            {
                var ownerParticipants = await _participantService.GetBySessionIdAsync(sessionId);

                var ownerResult = _mapper.Map<IReadOnlyCollection<ParticipantPublicListResponseDto>>(ownerParticipants);

                return Ok(ownerResult);
            }

            bool isAllowed = false;

            // belső user
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

            var result = _mapper.Map<IReadOnlyCollection<ParticipantPublicListResponseDto>>(participants);

            return Ok(result);
        }

        [HttpDelete("{sessionId}/{participantId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ParticipantPublicListResponseDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParticipantFromSessionById([FromRoute] string sessionId, [FromRoute] string participantId)
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
